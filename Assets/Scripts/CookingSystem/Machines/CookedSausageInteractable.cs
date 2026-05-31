using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

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
        if (!CanInteract())
            return;

        Grabbable item = _inventoryService.GetGrabbableInInventory();

        _inventoryService.RemoveItem(false);

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Destroy(item.gameObject);
        Destroy(gameObject);

        Grabbable frenchDog = Instantiate(
            frenchDogPrefab,
            spawnPosition,
            spawnRotation);

        _grabbablesComponent.RegisterNewGrabbable(frenchDog);
        frenchDog.TryGrab();

        Debug.Log("Френчдог собран");
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

        return IsBun(item);
    }

    private bool IsBun(Grabbable grabbable)
    {
        if (grabbable.TryGetComponent(out FoodItem foodItem))
            return foodItem.Type == FoodType.Bun;

        foodItem = grabbable.GetComponentInChildren<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Bun;

        foodItem = grabbable.GetComponentInParent<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Bun;

        return false;
    }
}