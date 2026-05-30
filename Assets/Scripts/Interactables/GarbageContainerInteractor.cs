using UnityEngine;
using Interactables.Interface;
using Unity.VisualScripting;
using System;

public class GarbageContainerInteractor : MonoBehaviour, IInteractor
{
    private GameObject garbageBag;
    public event Action garbageThrown;

    public void PerformInteraction(IInteractable interactable)
    {
        if (garbageBag == null)
            return;
        
        garbageThrown?.Invoke();

        Rigidbody rb = garbageBag.GetComponent<Rigidbody>();

        if (rb != null) 
            rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("GarbageBag"))
            return;
        
        if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactableGarbageBag))
        {
            Debug.Log("GarbageBag detected");

            garbageBag = collision.gameObject;

            PerformInteraction(interactableGarbageBag);
        }
    }

    public void EmptyContainer()
    {
        if (garbageBag != null)
        {
            Destroy(garbageBag);
            garbageBag = null;
        }
    }
}
