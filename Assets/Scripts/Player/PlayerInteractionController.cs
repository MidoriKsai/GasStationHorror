using Interactables.Interface;
using Player;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionController: MonoBehaviour
    {
        [SerializeField] private InteractionRaycastComponent interactionRaycastComponent;

        private void Update()
        {
            HandleInteraction();
        }

        private void HandleInteraction()
        {
            if (!Input.GetKeyDown(KeyCode.E))
                return;

            IInteractable interactable = interactionRaycastComponent.CurrentInteractable;

            if (interactable == null)
            {
                return;
            }

            if (!interactable.CanInteract())
            {
                return;
            }

            interactable.Interact();
        }
    }
}
