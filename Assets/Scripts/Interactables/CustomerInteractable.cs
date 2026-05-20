using Components;
using Cysharp.Threading.Tasks;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

namespace Interactables
{
    public class CustomerInteractable : MonoBehaviour, IInteractable
    {
        private CustomerData _data;
        private IInventoryService _inventory;
        private TipsSystem.TipController _tipController;

        private int _remainingCoffee;
        private int _remainingFrenchDogs;

        private UniTaskCompletionSource _tcs;

        public void Initialize(
            CustomerData data,
            IInventoryService inventory,
            TipsSystem.TipController tipController)
        {
            _data = data;
            _inventory = inventory;
            _tipController = tipController;

            _remainingCoffee = data.coffeeCount;
            _remainingFrenchDogs = data.frenchDogCount;

            Debug.Log($"[CustomerInteractable] Initialized. Coffee: {_remainingCoffee}, FrenchDogs: {_remainingFrenchDogs}");

            _tcs = new UniTaskCompletionSource();

            CheckComplete();
        }

        public void Interact()
        {
            if (_data == null)
            {
                Debug.Log("[CustomerInteractable] Interact blocked: _data == null");
                return;
            }

            if (_inventory == null)
            {
                Debug.Log("[CustomerInteractable] Interact blocked: _inventory == null");
                return;
            }

            if (_remainingCoffee <= 0 && _remainingFrenchDogs <= 0)
            {
                Debug.Log("[CustomerInteractable] Interact blocked: order already complete");
                return;
            }

            if (_inventory.IsInventoryEmpty())
            {
                Debug.Log("[CustomerInteractable] Interact blocked: inventory is empty");
                return;
            }

            Grabbable item = _inventory.GetGrabbableInInventory();

            if (item == null)
            {
                Debug.Log("[CustomerInteractable] Interact blocked: item == null");
                return;
            }

            if (!TryGetFoodItem(item, out FoodItem food))
            {
                Debug.Log("[CustomerInteractable] Interact blocked: item has no FoodItem");
                _tipController?.ShowTip("customer_not_needed_info");
                return;
            }

            Debug.Log($"[CustomerInteractable] Item in hand: {food.Type}. Coffee left: {_remainingCoffee}, FrenchDogs left: {_remainingFrenchDogs}");

            switch (food.Type)
            {
                case FoodType.Coffee:
                    GiveCoffee(item);
                    break;

                case FoodType.FrenchDog:
                    GiveFrenchDog(item);
                    break;

                default:
                    Debug.Log($"[CustomerInteractable] Item not needed: {food.Type}");
                    _tipController?.ShowTip("customer_not_needed_info");
                    break;
            }
        }

        public bool CanInteract()
        {
            if (_data == null)
                return false;

            if (_inventory == null)
                return false;

            if (_remainingCoffee <= 0 && _remainingFrenchDogs <= 0)
                return false;

            if (_inventory.IsInventoryEmpty())
                return false;

            Grabbable item = _inventory.GetGrabbableInInventory();

            if (item == null)
                return false;

            if (!TryGetFoodItem(item, out FoodItem food))
                return false;

            if (food.Type == FoodType.Coffee)
                return _remainingCoffee > 0;

            if (food.Type == FoodType.FrenchDog)
                return _remainingFrenchDogs > 0;

            return false;
        }

        public UniTask WaitOrderCompleted()
        {
            if (_tcs == null)
                return UniTask.CompletedTask;

            return _tcs.Task;
        }

        private void GiveCoffee(Grabbable item)
        {
            if (_remainingCoffee <= 0)
            {
                Debug.Log("[CustomerInteractable] Coffee rejected: no coffee needed");
                _tipController?.ShowTip("customer_not_needed_info");
                return;
            }

            AcceptItem(item);

            _remainingCoffee--;

            Debug.Log($"[CustomerInteractable] Coffee accepted. Remaining coffee: {_remainingCoffee}");

            CheckComplete();
        }

        private void GiveFrenchDog(Grabbable item)
        {
            if (_remainingFrenchDogs <= 0)
            {
                Debug.Log("[CustomerInteractable] FrenchDog rejected: no french dogs needed");
                _tipController?.ShowTip("customer_not_needed_info");
                return;
            }

            AcceptItem(item);

            _remainingFrenchDogs--;

            Debug.Log($"[CustomerInteractable] FrenchDog accepted. Remaining french dogs: {_remainingFrenchDogs}");

            CheckComplete();
        }

        private void AcceptItem(Grabbable item)
        {
            if (_inventory != null)
                _inventory.RemoveItem(true);

            if (item != null)
                Destroy(item.gameObject);
        }

        private bool TryGetFoodItem(Grabbable grabbable, out FoodItem foodItem)
        {
            foodItem = null;

            if (grabbable == null)
                return false;

            if (grabbable.TryGetComponent(out foodItem))
                return true;

            foodItem = grabbable.GetComponentInChildren<FoodItem>();

            if (foodItem != null)
                return true;

            foodItem = grabbable.GetComponentInParent<FoodItem>();

            return foodItem != null;
        }

        private void CheckComplete()
        {
            Debug.Log($"[CustomerInteractable] CheckComplete. Coffee: {_remainingCoffee}, FrenchDogs: {_remainingFrenchDogs}");

            if (_remainingCoffee <= 0 && _remainingFrenchDogs <= 0)
            {
                Debug.Log("[CustomerInteractable] Order completed");
                _tcs?.TrySetResult();
            }
        }
    }
}