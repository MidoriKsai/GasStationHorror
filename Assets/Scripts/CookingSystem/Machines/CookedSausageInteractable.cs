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
        Debug.Log("CookedSausage Interact");

        if (_inventoryService == null)
        {
            Debug.LogError("InventoryService is null");
            return;
        }

        if (_grabbablesComponent == null)
        {
            Debug.LogError("GrabbablesComponent is null");
            return;
        }

        if (_inventoryService.IsInventoryEmpty())
        {
            Debug.Log("Нужна булка");
            return;
        }

        var item = _inventoryService.GetGrabbableInInventory();

        if (item == null)
        {
            Debug.LogError("Item in hand is null");
            return;
        }

        if (!item.TryGetComponent<FoodItem>(out var food))
        {
            Debug.Log("В руке предмет без FoodItem");
            return;
        }

        Debug.Log($"В руке: {food.Type}");

        if (food.Type != FoodType.Bun)
        {
            Debug.Log("Нужна булка");
            return;
        }

        _inventoryService.RemoveItem(false);

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Object.Destroy(item.gameObject);
        Object.Destroy(gameObject);

        var frenchDog = Object.Instantiate(
            frenchDogPrefab,
            spawnPosition,
            spawnRotation);

        _grabbablesComponent.RegisterNewGrabbable(frenchDog);
        frenchDog.TryGrab();

        Debug.Log("Френчдог собран");
    }

    public bool CanInteract() => true;
}