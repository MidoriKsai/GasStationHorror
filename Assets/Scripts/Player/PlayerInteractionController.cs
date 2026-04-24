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
        }

        private void HandleInteraction()
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

            InteractionEvent?.Invoke(interactable);

            interactable.Interact();
        }
    }
}
