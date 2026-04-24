using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace NPCSystem.Manager
{
    public class ManagerTriggerZone: MonoBehaviour
    {
        private UniTaskCompletionSource _tcs;
        private bool _isTriggered = false;

        public UniTask WaitPlayerEnter()
        {
            if (_isTriggered)
                return UniTask.CompletedTask;

            _tcs = new UniTaskCompletionSource();
            return _tcs.Task;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerDataHandler>(out _))
                return;

            _isTriggered = true;
            _tcs?.TrySetResult();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<PlayerDataHandler>(out _))
                return;

            _isTriggered = false;
        }
    }
}