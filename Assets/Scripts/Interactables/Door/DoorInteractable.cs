using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interactables.Interface;
using UnityEngine;
using Utils;

namespace Interactables
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform doorTransform;
        [SerializeField] private Vector3 closedRotation;
        [SerializeField] private Vector3 openedRotation;
        [SerializeField] private float rotateDuration = 0.5f;
        [SerializeField] private Ease rotateEase = Ease.InOutSine;
        [SerializeField] private Collider doorCollider;
        [Header("Audio")]
        [SerializeField] private bool needMelodySound = false;
        [SerializeField] private AudioClip openDoorSound;
        [SerializeField] private AudioClip mainDoorOpenMelodySound;
        [SerializeField] private AudioSource doorAudioSource;

        private bool _isOpened;
        private bool _isAnimating;

        private CancellationTokenSource _animationCts;

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
            if (_isOpened)
                TryCloseDoor();
            else
                TryOpenDoor();
        }

        public void TryOpenDoor()
        {
            if(_isOpened)
                return;

            _animationCts?.Cancel();
            _animationCts = new CancellationTokenSource();
            OpenDoorAsync(_animationCts.Token).Forget();
        }

        public void TryCloseDoor()
        {
            if (!_isOpened)
                return;

            _animationCts?.Cancel();
            _animationCts = new CancellationTokenSource();
            CloseDoorAsync(_animationCts.Token).Forget();
        }

        private async UniTask OpenDoorAsync(CancellationToken ct)
        {
            doorCollider.enabled = false;
            _isAnimating = true;
            _isOpened = true;

            doorAudioSource.volume = 0.1f;
            doorAudioSource.PlayOneShot(openDoorSound);

            await doorTransform
                .DOLocalRotate(openedRotation, rotateDuration)
                .SetEase(rotateEase)
                .AwaitAsync(ct);

            if (needMelodySound)
            {
                doorAudioSource.volume = 0.2f;
                doorAudioSource.PlayOneShot(mainDoorOpenMelodySound);
            }

            doorCollider.enabled = true;
            _isAnimating = false;
        }

        private async UniTask CloseDoorAsync(CancellationToken ct)
        {
            doorCollider.enabled = false;
            _isAnimating = true;
            _isOpened = false;

            doorAudioSource.volume = 0.1f;
            doorAudioSource.PlayOneShot(openDoorSound);

            await doorTransform
                .DOLocalRotate(closedRotation, rotateDuration)
                .SetEase(rotateEase)
                .AwaitAsync(ct);

            _isAnimating = false;
            doorCollider.enabled = true;
        }
    }
}