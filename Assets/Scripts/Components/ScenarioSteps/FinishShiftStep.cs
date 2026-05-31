using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Interactables.Car;
using UnityEngine;

namespace Components.ScenarioSteps
{
    public class FinishShiftStep : BaseStep
    {
        [SerializeField] private CarInteractable carInteractable;
        
        [SerializeField] private GameObject finishShiftPanel;
        
        [SerializeField] private bool hidePanelOnInitialize = true;
        [SerializeField] private bool hidePanelOnStepStart = true;
        [SerializeField] private bool disableInteractAfterClick = true;

        private UniTaskCompletionSource _interactTcs;
        private bool _isStepActive;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            if (hidePanelOnInitialize && finishShiftPanel != null)
                finishShiftPanel.SetActive(false);

            if (carInteractable != null)
            {
                carInteractable.Initialize(OnCarInteractClicked);
                carInteractable.SetAvailable(false);
            }
        }

        public override async UniTask PerformStepAsync(CancellationToken ct)
        {
            if (carInteractable == null)
            {
                return;
            }

            if (finishShiftPanel == null)
            {
                return;
            }

            _isStepActive = true;
            _interactTcs = new UniTaskCompletionSource();

            if (hidePanelOnStepStart)
                finishShiftPanel.SetActive(false);

            carInteractable.SetAvailable(true);

            try
            {
                await _interactTcs.Task.AttachExternalCancellation(ct);
            }
            finally
            {
                _isStepActive = false;

                if (disableInteractAfterClick && carInteractable != null)
                    carInteractable.SetAvailable(false);
            }
        }

        private void OnCarInteractClicked()
        {
            if (!_isStepActive)
                return;

            if (finishShiftPanel != null)
                finishShiftPanel.SetActive(true);

            _interactTcs?.TrySetResult();
        }

        private void OnDestroy()
        {
            _isStepActive = false;

            if (carInteractable != null)
                carInteractable.SetAvailable(false);

            _interactTcs?.TrySetCanceled();
        }
    }
}