using Components;
using Interactables.Interface;
using Services.Interfaces;
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
            inventoryService.RemoveItem(needToAddForce: false);
        }
        else
        {
            inventoryService.AddItem(garbagePrefab);
        }
    }

    void Start()
    {
        inventoryService = grabbablesComponent.GetInventoryService();
    }
}
