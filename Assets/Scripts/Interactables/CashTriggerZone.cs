using Cysharp.Threading.Tasks;
using UnityEngine;

public class CashTriggerZone : MonoBehaviour
{
    private UniTaskCompletionSource _tcs;

    public UniTask WaitPlayerEnter()
    {
        _tcs = new UniTaskCompletionSource();
        return _tcs.Task;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player enter cash trigger");

        if (!other.CompareTag("Player"))
            return;

        _tcs?.TrySetResult();
    }
}