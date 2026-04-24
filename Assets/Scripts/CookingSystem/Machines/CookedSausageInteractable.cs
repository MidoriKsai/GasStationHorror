using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;
using Components;

public class CookedSausageInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable frenchDogPrefab;
    
    private IInventoryService _inventoryService;
    private GrabbablesComponent _grabbablesComponent;

    public void Initialize(
        IInventoryService inventoryService,
        GrabbablesComponent grabbablesComponent)
    {
        _inventoryService = inventoryService;
        _grabbablesComponent = grabbablesComponent;
    }

    public void Interact()
    {
        if (_inventoryService.IsInventoryEmpty())
        {
            Debug.Log("Нужна булка");
            return;
        }

        var item = _inventoryService.GetGrabbableInInventory();

        if (!item.TryGetComponent<FoodItem>(out var food))
            return;

        if (food.Type != FoodType.Bun)
        {
            Debug.Log("Нужна булка");
            return;
        }

        _inventoryService.RemoveItem(false);

        Object.Destroy(item.gameObject);
        Object.Destroy(gameObject);

        var frenchDog = Object.Instantiate(
            frenchDogPrefab,
            transform.position,
            transform.rotation);
        
        _grabbablesComponent.RegisterNewGrabbable(frenchDog);
                frenchDog.TryGrab();
    }

    public bool CanInteract() => true;
}