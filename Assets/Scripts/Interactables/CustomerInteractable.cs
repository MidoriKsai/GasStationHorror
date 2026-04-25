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

        private int _remainingCoffee;
        private int _remainingFrenchDogs;

        private UniTaskCompletionSource _tcs;

        public void Initialize(CustomerData data, IInventoryService inventory)
        {
            _data = data;
            _inventory = inventory;

            _remainingCoffee = data.coffeeCount;
            _remainingFrenchDogs = data.frenchDogCount;

            Debug.Log(data.coffeeCount);
            Debug.Log(data.frenchDogCount);

            _tcs = new UniTaskCompletionSource();

            CheckComplete();
        }

        public void Interact()
        {
            if (_data == null || _inventory == null)
            {
                Debug.LogError("CustomerInteractable не инициализирован");
                return;
            }

            if (_inventory.IsInventoryEmpty())
            {
                return;
            }

            var item = _inventory.GetGrabbableInInventory();

            if (!item.TryGetComponent<FoodItem>(out var food))
                return;

            switch (food.Type)
            {
                case FoodType.Coffee:
                    GiveCoffee(item);
                    break;

                case FoodType.FrenchDog:
                    GiveFrenchDog(item);
                    break;
            }
        }

        public bool CanInteract()
        {
            return _data != null &&
                   (_remainingCoffee > 0 || _remainingFrenchDogs > 0);
        }

        public UniTask WaitOrderCompleted()
        {
            if (_tcs == null)
            {
                return UniTask.CompletedTask;
            }

            return _tcs.Task;
        }

        private void GiveCoffee(Grabbable item)
        {
            if (_remainingCoffee <= 0) return;

            _remainingCoffee--;

            _inventory.RemoveItem(true);
            Destroy(item.gameObject);

            CheckComplete();
        }

        private void GiveFrenchDog(Grabbable item)
        {
            if (_remainingFrenchDogs <= 0) return;

            _remainingFrenchDogs--;

            _inventory.RemoveItem(true);
            Destroy(item.gameObject);

            CheckComplete();
        }

        private void CheckComplete()
        {
            if (_remainingCoffee <= 0 && _remainingFrenchDogs <= 0)
            {
                _tcs?.TrySetResult();
            }
        }
    }
}