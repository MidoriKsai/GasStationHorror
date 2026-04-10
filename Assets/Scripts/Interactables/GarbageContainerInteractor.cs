using UnityEngine;
using Interactables.Interface;

public class GarbageContainerInteractor : MonoBehaviour, IInteractor
{
    private GameObject garbageBag;

    public void PerformInteraction(IInteractable interactableGarbageBag)
    {
        Debug.Log("GarbageBag detected");

        Rigidbody rb = garbageBag.GetComponent<Rigidbody>();

        if (rb != null) 
            rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactableGarbageBag))
        {
            garbageBag = collision.gameObject;
            PerformInteraction(interactableGarbageBag);
        }
    }
}
