using System.Collections.Generic;
using DG.Tweening;
using Interactables.Interface;
using UnityEngine;

namespace Interactables
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform doorTransform;
        [SerializeField] private Vector3 closedRotation;
        [SerializeField] private Vector3 openedRotation;
        [SerializeField] private float rotateDuration = 0.5f;
        [SerializeField] private Ease rotateEase = Ease.InOutSine;

        private readonly HashSet<Component> _activeUsers = new();

        private bool _isOpened;
        private bool _isAnimating;
        private Tween _rotateTween;

        private void Start()
        {
            if (doorTransform == null) 
                return;


            doorTransform.localRotation = Quaternion.Euler(closedRotation);
            _isOpened = false;
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            if (_activeUsers.Count > 0)
                return;

            if (_isOpened)
                CloseDoor();
            else
                OpenDoor();
        }

        public void RegisterUser(Component user)
        {
            if (user == null)
                return;

            bool wasAdded = _activeUsers.Add(user);

            if (!wasAdded)
                return;

            if (!_isOpened || _isAnimating)
                OpenDoor();
        }

        public void UnregisterUser(Component user)
        {
            if (user == null)
                return;

            _activeUsers.Remove(user);

            if (_activeUsers.Count == 0)
                CloseDoor();
        }

        private void OpenDoor()
        {
            _rotateTween?.Kill();

            _isAnimating = true;
            _isOpened = true;

            _rotateTween = doorTransform
                .DOLocalRotate(openedRotation, rotateDuration)
                .SetEase(rotateEase)
                .OnComplete(OnTweenCompleted);
        }

        private void CloseDoor()
        {
            _rotateTween?.Kill();

            _isAnimating = true;
            _isOpened = false;

            _rotateTween = doorTransform
                .DOLocalRotate(closedRotation, rotateDuration)
                .SetEase(rotateEase)
                .OnComplete(OnTweenCompleted);
        }

        private void OnTweenCompleted()
        {
            _isAnimating = false;
        }

        private void OnDestroy()
        {
            _rotateTween?.Kill();
        }
    }
}