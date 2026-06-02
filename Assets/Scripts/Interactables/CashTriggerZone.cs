using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

public class CashTriggerZone : MonoBehaviour
{
    private UniTaskCompletionSource _tcs;
    private bool _isPlayerInside;

    public UniTask WaitPlayerEnter()
    {
        if (_isPlayerInside)
            return UniTask.CompletedTask;

        _tcs = new UniTaskCompletionSource();
        return _tcs.Task;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerDataHandler>(out _))
            return;

        _isPlayerInside = true;
        _tcs?.TrySetResult();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isPlayerInside)
            return;

        if (!other.TryGetComponent<PlayerDataHandler>(out _))
            return;

        _isPlayerInside = true;
        _tcs?.TrySetResult();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerDataHandler>(out _))
            return;

        _isPlayerInside = false;
    }
}