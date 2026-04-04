using System.Threading;
using Controllers;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class CustomerStep : BaseStep
    {
        private CustomerController _customer;
        private DialogueSystemController _dialogue;
        private PlayerMovement _playerMovement;
        private CameraLook _cameraLook;

        [SerializeField] private MoneyView moneyPrefab;
        [SerializeField] private Transform moneySpawnPoint;
        [SerializeField] private CashTriggerZone cashTriggerZone;
        [SerializeField] private Transform _dialogueLookTarget;

        public override void Initialize(ServiceContainer container)
        {
            _customer = container.Resolve<CustomerController>();
            _dialogue = container.Resolve<DialogueSystemController>();
            
            var player = GameObject.FindWithTag("PlayerParent");
            if (player != null)
                Debug.Log("Player finded");
            var components = player.GetComponents<Component>();

            foreach (var component in components)
            {
                Debug.Log(component.GetType().Name);
            }
            _playerMovement = player.GetComponent<PlayerMovement>();
            _cameraLook = player.GetComponent<CameraLook>();
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            await _customer.WaitForCustomerArriveAsync();

            await cashTriggerZone.WaitPlayerEnter();
            
            Debug.Log(_playerMovement == null ? "PlayerMovement not found" : "PlayerMovement found");
            Debug.Log(_cameraLook == null ? "CameraLook not found" : "CameraLook found");
            _playerMovement.SetMovementEnabled(false);
            _cameraLook.SnapToTarget(_dialogueLookTarget);
            _cameraLook.SetLookEnabled(false);

            await _dialogue.StartDialogueAsync("customer_intro", ct);
            
            _playerMovement.SetMovementEnabled(true);
            _cameraLook.SetLookEnabled(true);

            await ScanItems();

            await WaitPayment();

            await _customer.CustomerLeaveAsync();
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