using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class ReadyCoffeeInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable finalCoffeePrefab;

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
        if (!CanInteract())
            return;

        Grabbable lid = _inventoryService.GetGrabbableInInventory();

        _inventoryService.RemoveItem(false);

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Destroy(lid.gameObject);
        Destroy(gameObject);

        Grabbable finalCoffee = Instantiate(
            finalCoffeePrefab,
            spawnPosition,
            spawnRotation);

        _grabbablesComponent.RegisterNewGrabbable(finalCoffee);
        finalCoffee.TryGrab();
    }

    public bool CanInteract()
    {
        if (_inventoryService == null)
            return false;

        if (_grabbablesComponent == null)
            return false;

        if (_inventoryService.IsInventoryEmpty())
            return false;

        Grabbable item = _inventoryService.GetGrabbableInInventory();

        if (item == null)
            return false;

        return IsCoffeeLid(item);
    }

    private bool IsCoffeeLid(Grabbable grabbable)
    {
        if (grabbable == null)
            return false;

        if (grabbable.TryGetComponent(out FoodItem foodItem))
            return foodItem.Type == FoodType.Lid;

        foodItem = grabbable.GetComponentInChildren<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Lid;

        foodItem = grabbable.GetComponentInParent<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Lid;

        return false;
    }
}