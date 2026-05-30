using Components;
using Interactables.Interface;
using Services.Interfaces;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UrnInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GrabbablesComponent grabbablesComponent;
    [SerializeField] private Grabbable garbagePrefab;
    private IInventoryService inventoryService;


    public bool CanInteract()
        => true;

    public void Interact()
    {
        if (!inventoryService.IsInventoryEmpty())
        {
            Grabbable grabbable = inventoryService.GetGrabbableInInventory();
            inventoryService.RemoveItem(needToAddForce: false);
            Destroy(grabbable.gameObject);
        }
        else
        {
            Grabbable garbage = Instantiate(garbagePrefab, transform.position, Quaternion.identity);

            grabbablesComponent.RegisterNewGrabbable(garbage);
            garbage.Interact();
        }
    }

    void Start()
    {
        inventoryService = grabbablesComponent.GetInventoryService();
    }
}
