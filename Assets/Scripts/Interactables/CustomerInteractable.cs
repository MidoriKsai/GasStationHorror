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

            _remainingCoffee =
                data.coffeeCount;

            _remainingFrenchDogs =
                data.frenchDogCount;

            _data.readyCoffee = 0;

            _data.readyFrenchDogs = 0;

            _tcs =
                new UniTaskCompletionSource();

            RefreshInfo();

            CheckComplete();
        }

        public void Interact()
        {
            if (_data == null)
                return;

            if (_inventory == null)
                return;

            if (_inventory.IsInventoryEmpty())
                return;

            Grabbable item =
                _inventory.GetGrabbableInInventory();

            if (item == null)
                return;

            if (!TryGetFoodItem(item, out FoodItem food))
            {
                _tipController?.ShowPopup(
                    "customer_not_needed_info");

                return;
            }

            switch (food.Type)
            {
                case FoodType.Coffee:
                    GiveCoffee(item);
                    break;

                case FoodType.FrenchDog:
                    GiveFrenchDog(item);
                    break;

                default:
                    _tipController?.ShowPopup(
                        "customer_not_needed_info");
                    break;
            }
        }

        public bool CanInteract()
        {
            if (_data == null)
                return false;

            if (_inventory == null)
                return false;

            if (_inventory.IsInventoryEmpty())
                return false;

            Grabbable item =
                _inventory.GetGrabbableInInventory();

            if (item == null)
                return false;

            if (!TryGetFoodItem(item, out FoodItem food))
                return false;

            if (food.Type == FoodType.Coffee)
                return true;

            if (food.Type == FoodType.FrenchDog)
                return true;

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
                _tipController?.ShowPopup(
                    "customer_extra_food");

                return;
            }

            AcceptItem(item);

            _remainingCoffee--;

            _data.readyCoffee++;

            RefreshInfo();

            CheckComplete();
        }

        private void GiveFrenchDog(Grabbable item)
        {
            if (_remainingFrenchDogs <= 0)
            {
                _tipController?.ShowPopup(
                    "customer_extra_food");

                return;
            }

            AcceptItem(item);

            _remainingFrenchDogs--;

            _data.readyFrenchDogs++;

            RefreshInfo();

            CheckComplete();
        }

        private void RefreshInfo()
        {
            _tipController?.ShowInfo(
                "customer_info",
                _data);
        }

        private void AcceptItem(Grabbable item)
        {
            _inventory?.RemoveItem(true);

            if (item != null)
                Destroy(item.gameObject);
        }

        private bool TryGetFoodItem(
            Grabbable grabbable,
            out FoodItem foodItem)
        {
            foodItem = null;

            if (grabbable == null)
                return false;

            if (grabbable.TryGetComponent(out foodItem))
                return true;

            foodItem =
                grabbable.GetComponentInChildren<FoodItem>();

            if (foodItem != null)
                return true;

            foodItem =
                grabbable.GetComponentInParent<FoodItem>();

            return foodItem != null;
        }

        private void CheckComplete()
        {
            if (_remainingCoffee <= 0 &&
                _remainingFrenchDogs <= 0)
            {
                _tcs?.TrySetResult();
            }
        }
    }
}