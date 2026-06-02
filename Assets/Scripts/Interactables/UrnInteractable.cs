using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class UrnInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GrabbablesComponent grabbablesComponent;

    [SerializeField] private Grabbable garbagePrefab;

    private IInventoryService inventoryService;

    private TipsSystem.TipController _tipController;

    private bool _canTakeGarbage;

    public void Initialize(
        IInventoryService inventory,
        TipsSystem.TipController tipController)
    {
        inventoryService = inventory;

        _tipController = tipController;

        _canTakeGarbage = false;
    }

    public void SetCanTakeGarbage(bool value)
    {
        _canTakeGarbage = value;
    }

    public bool CanInteract()
        => true;

    public void Interact()
    {
        if (inventoryService == null)
            return;
        
        if (!inventoryService.IsInventoryEmpty())
        {
            Grabbable grabbable =
                inventoryService.GetGrabbableInInventory();

            if (grabbable == null)
                return;

            if (!CanThrow(grabbable.gameObject))
                return;

            inventoryService.RemoveItem(false);

            Destroy(grabbable.gameObject);

            return;
        }
        
        if (!_canTakeGarbage)
        {
            _tipController?.ShowPopup(
                "trash_empty_hands");

            return;
        }
        
        Grabbable garbage =
            Instantiate(
                garbagePrefab,
                transform.position,
                Quaternion.identity);

        grabbablesComponent.RegisterNewGrabbable(garbage);

        garbage.Interact();
    }

    private bool CanThrow(GameObject obj)
    {
        Trashable trashable =
            obj.GetComponent<Trashable>();
        
        if (obj.CompareTag("GarbageBag"))
        {
            _tipController?.ShowPopup("trash_bag_street");
            return false;
        }

        if (trashable == null)
            trashable =
                obj.GetComponentInChildren<Trashable>();

        if (trashable == null)
            return true;

        if (!trashable.CanBeThrownAway)
        {
            _tipController?.ShowPopup(
                "trash_customer_item");

            return false;
        }

        return true;
    }
}