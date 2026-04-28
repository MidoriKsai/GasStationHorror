using UnityEngine;
using Interactables.Interface;
using Components;
using Services.Interfaces;

public class GarbageDispenserBin : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable dispensableGrabbable;
    [SerializeField] private Transform holdingPoint;
    [SerializeField] private GrabbablesComponent grabbablesComponent;
    private IInventoryService inventoryService;

    void Start()
    {
        inventoryService = grabbablesComponent.GetInventoryService();
    }

    public void Interact()
    {
        Grabbable dispensedGrabbable = Instantiate(dispensableGrabbable, transform.position, Quaternion.identity);

        grabbablesComponent.RegisterNewGrabbable(dispensedGrabbable);
        dispensedGrabbable.Interact();
    }

    public bool CanInteract()
        => inventoryService.IsInventoryEmpty();
}
