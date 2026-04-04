using System.Threading;
using Controllers;
using Core;
using Cysharp.Threading.Tasks;
using Services.Interfaces;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class CustomerStep : BaseStep
    {
        private CustomerController _customer;
        private DialogueSystemController _dialogue;

        [SerializeField] private MoneyView moneyPrefab;
        [SerializeField] private Transform moneySpawnPoint;
        [SerializeField] private CashTriggerZone cashTriggerZone;
        [SerializeField] private Transform _dialogueLookTarget;

        private IPlayerService _playerService;

        public override void Initialize(ServiceContainer container)
        {
            _customer = container.Resolve<CustomerController>();
            _dialogue = container.Resolve<DialogueSystemController>();
            _playerService = container.Resolve<IPlayerService>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            await _customer.WaitForCustomerArriveAsync();

            await cashTriggerZone.WaitPlayerEnter();

            _playerService.FocusPlayerToDialogue(_dialogueLookTarget);

            await _dialogue.StartDialogueAsync("customer_intro", ct);

            _playerService.UnfocusPlayerFromDialogue();

            await ScanItems();

            await WaitPayment();

            await _customer.WaitForCustomerLeaveAsync();
        }

        private async UniTask ScanItems()
        {
            await UniTask.Delay(1000);
        }

        private async UniTask WaitPayment()
        {
            var tcs = new UniTaskCompletionSource();

            var money = Object.Instantiate(moneyPrefab, moneySpawnPoint);

            void OnClicked()
            {
                money.Clicked -= OnClicked;
                Object.Destroy(money.gameObject);
                tcs.TrySetResult();
            }

            money.Clicked += OnClicked;

            await tcs.Task;
        }
    }
}