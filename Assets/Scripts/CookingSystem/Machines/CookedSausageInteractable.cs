using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class CookedSausageInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable frenchDogPrefab;

    private IInventoryService _inventoryService;

    private GrabbablesComponent _grabbablesComponent;

    private TipsSystem.TipController _tipController;

    public void Initialize(
        IInventoryService inventoryService,
        GrabbablesComponent grabbablesComponent,
        TipsSystem.TipController tipController)
    {
        _inventoryService = inventoryService;

        _grabbablesComponent = grabbablesComponent;

        _tipController = tipController;
    }

    public void Interact()
    {
        if (_inventoryService == null)
            return;

        if (_inventoryService.IsInventoryEmpty())
        {
            _tipController?.ShowPopup(
                "grill_need_bun");

            return;
        }

        Grabbable item =
            _inventoryService.GetGrabbableInInventory();

        if (item == null)
        {
            _tipController?.ShowPopup(
                "grill_need_bun");

            return;
        }

        if (!IsBun(item))
        {
            _tipController?.ShowPopup(
                "grill_need_bun");

            return;
        }

        _inventoryService.RemoveItem(false);

        Vector3 spawnPosition =
            transform.position;

        Quaternion spawnRotation =
            transform.rotation;

        Destroy(item.gameObject);

        Destroy(gameObject);

        Grabbable frenchDog = Instantiate(
            frenchDogPrefab,
            spawnPosition,
            spawnRotation);

        _grabbablesComponent.RegisterNewGrabbable(
            frenchDog);

        frenchDog.TryGrab();

        Debug.Log("Френчдог собран");
    }

    public bool CanInteract()
    {
        return true;
    }

    private bool IsBun(Grabbable grabbable)
    {
        if (grabbable == null)
            return false;

        if (grabbable.TryGetComponent(
                out FoodItem foodItem))
        {
            return foodItem.Type ==
                   FoodType.Bun;
        }

        foodItem =
            grabbable.GetComponentInChildren<FoodItem>();

        if (foodItem != null)
        {
            return foodItem.Type ==
                   FoodType.Bun;
        }

        foodItem =
            grabbable.GetComponentInParent<FoodItem>();

        if (foodItem != null)
        {
            return foodItem.Type ==
                   FoodType.Bun;
        }

        return false;
    }
}