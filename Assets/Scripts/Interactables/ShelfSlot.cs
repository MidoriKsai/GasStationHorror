using UnityEngine;
using Interactables.Interface;
using Components;
using Services.Interfaces;
using System;

public class ShelfSlot : MonoBehaviour, IInteractable
{

    private GrabbablesComponent grabbablesComponent;
    private IInventoryService inventoryService;
    public event Action OnFilledEvent;
    private bool canInteract = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        inventoryService = grabbablesComponent.GetInventoryService();
    }

    public void Interact()
    {
        if (!inventoryService.IsInventoryEmpty())
        {
            Grabbable grabbable = inventoryService.GetGrabbableInInventory();
            inventoryService.RemoveItem(false);
            grabbable.transform.SetParent(transform);
            grabbable.transform.position = transform.position;
            grabbable.ResetLocalRotation();

            canInteract = false;
            OnFilledEvent?.Invoke();
        }
    }

    public void SetGrabbablesComponent(GrabbablesComponent grabbablesComponent)
    {
        this.grabbablesComponent = grabbablesComponent;
    }

    // Update is called once per frame
    public bool CanInteract()
        => canInteract;
}
