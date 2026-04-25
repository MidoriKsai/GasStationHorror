using System.Threading;
using Controllers;
using Core;
using Cysharp.Threading.Tasks;
using Interactables;
using Services.Interfaces;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class CustomerStep : BaseStep
    {
        private CustomerController _customerController;
        private DialogueSystemController _dialogue;
        private SmartTerminalController _terminalController;
        private TipsSystem.TipController _tipController;

        [SerializeField] private string dialogueID;
        [SerializeField] private MoneyView moneyPrefab;
        [SerializeField] private Transform moneySpawnPoint;
        [SerializeField] private CashTriggerZone cashTriggerZone;
        [SerializeField] private CustomerProductsHandler customerProductsHandler;

        private IPlayerService _playerService;
        private IInventoryService _inventoryService;
        private CustomerInteractable _customerInteractable;

        public override void Initialize(ServiceContainer container)
        {
            _customerController = container.Resolve<CustomerController>();
            _dialogue = container.Resolve<DialogueSystemController>();
            _playerService = container.Resolve<IPlayerService>();
            _terminalController = container.Resolve<SmartTerminalController>();
            _inventoryService = container.Resolve<IInventoryService>();
            _tipController = container.Resolve<TipsSystem.TipController>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            _terminalController.DisableInteraction();

            await _customerController.WaitForCustomerArriveAsync();

            _customerInteractable = _customerController
                .currentCustomer
                .GetComponent<CustomerInteractable>();

            _customerInteractable.Initialize(
                _customerController.currentCustomerData,
                _inventoryService);

            await cashTriggerZone.WaitPlayerEnter();

            _playerService.FocusPlayerToDialogue(_customerController.GetCustomerDialogPoint());

            await _dialogue.StartDialogueAsync(
                dialogueID,
                _customerController.currentCustomerData,
                ct);

            _playerService.UnfocusPlayerFromDialogue();
            

            customerProductsHandler.StartProducts();
            

            _tipController.ShowTipDelayed("terminal_info_false", 10f, ct);

            await customerProductsHandler.WaitAllScanned();
            
            _tipController.CancelDelayedTip();
            _tipController.HideTip();

            
            _terminalController.EnableInteraction(_customerController.currentCustomerData);

            await _terminalController.WaitForResultAsync();
            
            _tipController.HideTip();

            _terminalController.DisableInteraction();
            
            await _customerInteractable.WaitOrderCompleted();

            await WaitPayment();

            await _customerController.WaitForCustomerLeaveAsync();
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