using System;
using Interactables.Interface;
using TipsSystem;
using UnityEngine;

namespace Interactables.Car
{
    public class CarInteractable : MonoBehaviour, IInteractable
    {
        private Action _onInteract;

        private bool _isAvailable;

        private TipController _tipController;

        public void Initialize(
            Action onInteract,
            TipController tipController)
        {
            _onInteract = onInteract;

            _tipController = tipController;

            _isAvailable = false;
        }

        public void SetAvailable(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public void Interact()
        {
            if (_onInteract == null)
                return;

            if (!_isAvailable)
            {
                _tipController?.ShowPopup(
                    "shift_not_finished");

                return;
            }

            _onInteract?.Invoke();
        }

        public bool CanInteract()
        {
            return _onInteract != null;
        }
    }
}