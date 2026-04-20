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
        private CustomerController _customerController;
        private DialogueSystemController _dialogue;
        private SmartTerminalController _terminalController;


        [SerializeField] private MoneyView moneyPrefab;
        [SerializeField] private Transform moneySpawnPoint;
        [SerializeField] private CashTriggerZone cashTriggerZone;
        [SerializeField] private CustomerProductsHandler customerProductsHandler;

        private IPlayerService _playerService;

        public override void Initialize(ServiceContainer container)
        {
            _customerController = container.Resolve<CustomerController>();
            _dialogue = container.Resolve<DialogueSystemController>();
            _playerService = container.Resolve<IPlayerService>();
            _terminalController = container.Resolve<SmartTerminalController>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            await _customerController.WaitForCustomerArriveAsync();

            await cashTriggerZone.WaitPlayerEnter();

            _playerService.FocusPlayerToDialogue(_customerController.GetCustomerDialogPoint());

            await _dialogue.StartDialogueAsync("customer_intro", ct);

            _playerService.UnfocusPlayerFromDialogue();

            customerProductsHandler.StartProducts();

            await customerProductsHandler.WaitAllScanned();
            
            _terminalController.EnableInteraction(_customerController.currentCustomerData);

            bool terminalSuccess = await _terminalController.WaitForResultAsync();

            if (!terminalSuccess)
            {
                return;
            }

            await WaitPayment();

            await _customerController.WaitForCustomerLeaveAsync();
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