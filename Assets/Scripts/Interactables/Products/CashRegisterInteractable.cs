using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class CashRegisterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private CustomerProductsHandler customerProductsHandler;

    private IInventoryService _inventoryService;
    private bool _isAvailable;

    public void Initialize(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        _isAvailable = false;
    }

    public void SetAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        var grabbable = _inventoryService.GetGrabbableInInventory();

        bool success = customerProductsHandler.TryScan(grabbable);

        if (success)
        {
            _inventoryService.RemoveItem(false);
        }
    }

    public bool CanInteract()
    {
        if (!_isAvailable)
            return false;

        if (_inventoryService == null)
            return false;

        return !_inventoryService.IsInventoryEmpty();
    }
}