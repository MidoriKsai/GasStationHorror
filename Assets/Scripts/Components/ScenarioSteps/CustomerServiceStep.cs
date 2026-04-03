using System.Threading;
using Controllers;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class CustomerStep : BaseStep
    {
        private CustomerController customer;
        private DialogueSystemController dialogue;
        private CashTriggerZone trigger;
        private CustomerStepHandler customerStepHandler;

        [SerializeField] private MoneyView moneyPrefab;
        [SerializeField] private Transform moneySpawnPoint;


        public override void Initialize(ServiceContainer container)
        {
            customer = container.Resolve<CustomerController>();
            dialogue = container.Resolve<DialogueSystemController>();
            customerStepHandler = container.Resolve<CustomerStepHandler>();
            trigger = customerStepHandler.CashTriggerZone;
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            await customer.SpawnCustomerSequenceAsync();

            await trigger.WaitPlayerEnter();

            await dialogue.StartDialogueAsync("customer_intro", ct);

            await ScanItems();

            await WaitPayment();

            await customer.CustomerLeaveAsync();
        }

        private async UniTask ScanItems()
        {
            await UniTask.Delay(1000);
        }

        private async UniTask WaitPayment()
        {
            var tcs = new UniTaskCompletionSource();

            var money = Object.Instantiate(moneyPrefab, moneySpawnPoint);

            money.Clicked += () =>
            {
                Object.Destroy(money.gameObject);
                tcs.TrySetResult();
            };

            await tcs.Task;
        }
    }
}