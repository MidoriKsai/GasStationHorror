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
        [SerializeField] private CashRegisterInteractable cashRegisterInteractable;

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

            if (cashRegisterInteractable != null)
            {
                cashRegisterInteractable.Initialize(_inventoryService);
                cashRegisterInteractable.SetAvailable(false);
            }
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            _terminalController.DisableInteraction();

            if (cashRegisterInteractable != null)
                cashRegisterInteractable.SetAvailable(false);

            await _customerController.WaitForCustomerArriveAsync();

            _customerInteractable = _customerController
                .currentCustomer
                .GetComponent<CustomerInteractable>();

            _customerInteractable.Initialize(
                _customerController.currentCustomerData,
                _inventoryService,
                _tipController);

            _tipController.ShowObjective(
                "customer_start");

            await cashTriggerZone.WaitPlayerEnter();

            _tipController.HideObjective();

            _playerService.FocusPlayerToDialogue(
                _customerController.GetCustomerDialogPoint());

            await _dialogue.StartDialogueAsync(
                dialogueID,
                _customerController.currentCustomerData,
                ct);

            _playerService.UnfocusPlayerFromDialogue();
            

            customerProductsHandler.StartProducts();
            
            _tipController.ShowInfo(
                "customer_info",
                _customerController.currentCustomerData);

            _tipController.ShowObjective(
                "products_info");

            if (cashRegisterInteractable != null)
                cashRegisterInteractable.SetAvailable(true);

            await customerProductsHandler.WaitAllScanned();

            if (cashRegisterInteractable != null)
                cashRegisterInteractable.SetAvailable(false);

            _tipController.ShowObjective(
                "dog_info");

            await _customerInteractable.WaitOrderCompleted();

            _tipController.ShowObjective(
                "finish_info");

            _terminalController.EnableInteraction(
                _customerController.currentCustomerData);

            await _terminalController.WaitForResultAsync();

            _tipController.HideObjective();

            _tipController.HideInfo();

            _terminalController.DisableInteraction();

            _tipController.ShowObjective(
                "finish_money_info");

            customerProductsHandler.ClearProducts();

            await WaitPayment();

            _tipController.HideObjective();

            await _customerController.WaitForCustomerLeaveAsync();
        }

        private async UniTask WaitPayment()
        {
            var tcs = new UniTaskCompletionSource();

            var money = Object.Instantiate(
                moneyPrefab,
                moneySpawnPoint);

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