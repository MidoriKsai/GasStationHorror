using UnityEngine;
using Interactables.Interface;
using Components;
using Services.Interfaces;

public class ShelfSlot : MonoBehaviour, IInteractable
{

    [SerializeField] private GrabbablesComponent grabbablesComponent;
    private IInventoryService inventoryService;
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
            inventoryService.RemoveItem();
            grabbable.DisablePhysics();
            grabbable.transform.SetParent(transform);
            grabbable.transform.position = transform.position;

        }
    }

    // Update is called once per frame
    public bool CanInteract()
        => true;
}
