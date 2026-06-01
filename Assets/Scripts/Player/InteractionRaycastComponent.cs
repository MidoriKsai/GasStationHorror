using Interactables.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class InteractionRaycastComponent : MonoBehaviour
    {
        [SerializeField] private PlayerDataHandler playerDataHandler;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private Image handPointer;

        private Camera _camera;

        public IInteractable CurrentInteractable { get; private set; }

        private void Awake()
        {
            _camera = playerDataHandler.MainCamera;
        }

        private void Update()
        {
            UpdateCurrentInteractable();
        }

        private void UpdateCurrentInteractable()
        {
            CurrentInteractable = null;

            if (handPointer != null)
                handPointer.enabled = false;

            if (_camera == null)
                return;

            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
                return;

            if (!hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();

                if (interactable == null)
                    return;
            }

            if (!interactable.CanInteract())
                return;

            CurrentInteractable = interactable;

            if (handPointer != null)
                handPointer.enabled = true;
        }
    }
}