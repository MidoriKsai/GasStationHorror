using System;
using UnityEngine;
using Interactables.Interface;

public class GarbageContainerInteractor : MonoBehaviour, IInteractor
{
    [SerializeField] private string garbageTag = "GarbageBag";

    private GameObject garbageObject;
    private TipsSystem.TipController _tipController;

    public event Action garbageThrown;

    public void PerformInteraction(IInteractable interactable)
    {
        if (garbageObject == null)
            return;

        garbageThrown?.Invoke();

        Rigidbody rb = garbageObject.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject rootObject = collision.transform.root.gameObject;

        if (!rootObject.CompareTag(garbageTag))
            return;

        if (!CanThrow(rootObject))
            return;

        if (rootObject.TryGetComponent(out IInteractable interactable))
        {
            garbageObject = rootObject;

            PerformInteraction(interactable);
        }
    }

    private bool CanThrow(GameObject obj)
    {
        Debug.Log($"Checking object: {obj.name}");

        Trashable trashable = obj.GetComponent<Trashable>();

        if (trashable == null)
            trashable = obj.GetComponentInChildren<Trashable>();

        if (trashable == null)
            trashable = obj.GetComponentInParent<Trashable>();

        if (trashable == null)
        {
            Debug.Log("Trashable NOT FOUND");
            return false;
        }

        Debug.Log($"CanBeThrownAway = {trashable.CanBeThrownAway}");

        if (!trashable.CanBeThrownAway)
        {
            Debug.Log("ITEM CANNOT BE THROWN");

            _tipController?.ShowPopup("trash_customer_item");

            return false;
        }

        Debug.Log("ITEM CAN BE THROWN");

        return true;
    }

    public void EmptyContainer()
    {
        if (garbageObject != null)
        {
            Destroy(garbageObject);
            garbageObject = null;
        }
    }
}