using System;
using Interactables.Interface;
using Player;
using UnityEngine;

namespace Player
{
    public class PlayerInteractionController: MonoBehaviour
    {
        [SerializeField] private InteractionRaycastComponent interactionRaycastComponent;

        public IInteractable CurrentInteractable;

        public event Action<IInteractable> InteractionEvent;

        public event Action InteractionEnded;

        private void Update()
        {
            HandleInteraction();
            HandleContinuousInteraction();
        }

        private void HandleInteraction()
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }


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

        private void HandleContinuousInteraction()
        {
            if (!Input.GetKey(KeyCode.E))
            {
                InteractionEnded?.Invoke();
                CurrentInteractable = null;
                return;
            }

            IInteractable interactable = interactionRaycastComponent.CurrentInteractable;

            if (interactable == null)
            {
                CurrentInteractable = null;
                InteractionEnded?.Invoke();
                return;
            }

            if (!interactable.CanInteract())
            {
                CurrentInteractable = null;
                return;
            }

            CurrentInteractable = interactable;


            if (interactable is Mud)
            {
                interactable.Interact();

                InteractionEvent?.Invoke(interactable);
            }
        }
    }
}
