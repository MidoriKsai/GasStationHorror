using System;
using Interactables.Interface;
using UnityEngine;

namespace Interactables.Car
{
    public class CarInteractable : MonoBehaviour, IInteractable
    {
        private Action _onInteract;
        private bool _isAvailable;

        public void Initialize(Action onInteract)
        {
            _onInteract = onInteract;
            _isAvailable = false;
        }

        public void SetAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public void Interact()
        {
            if (!CanInteract())
                return;

            _onInteract?.Invoke();
        }

        public bool CanInteract()
        {
            return _onInteract != null && _isAvailable;
        }
    }
}