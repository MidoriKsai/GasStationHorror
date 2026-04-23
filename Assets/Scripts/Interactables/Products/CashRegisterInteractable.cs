using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class CashRegisterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private CustomerProductsHandler customerProductsHandler;

    private IInventoryService _inventoryService;

    public void Initialize(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public void Interact()
    {
        if (_inventoryService.IsInventoryEmpty())
        {
            Debug.Log("Инвентарь пуст");
            return;
        }

        var grabbable = _inventoryService.GetGrabbableInInventory();

        bool success = customerProductsHandler.TryScan(grabbable);

        if (success)
        {
            _inventoryService.RemoveItem(false);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}