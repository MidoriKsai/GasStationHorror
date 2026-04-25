using Interactables.Interface;
using TMPro;
using UnityEngine;

namespace Player
{
    public class InteractionRaycastComponent : MonoBehaviour
    {
        [SerializeField] private PlayerDataHandler playerDataHandler;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private TMP_Text interactionHelpText;

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
            interactionHelpText.enabled = false;

            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayerMask))
            {
                CurrentInteractable = hit.collider.GetComponentInParent<IInteractable>();
                interactionHelpText.enabled = true;
            }
        }
    }
}