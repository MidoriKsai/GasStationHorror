using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

public class CashTriggerZone : MonoBehaviour
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

        Debug.Log("Player enter cash trigger");

        _isTriggered = true;
        _tcs?.TrySetResult();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerDataHandler>(out _))
            return;

        Debug.Log("Player exit cash trigger");

        _isTriggered = false;
    }
}