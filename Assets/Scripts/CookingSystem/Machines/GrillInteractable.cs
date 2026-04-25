using Components;
using Cysharp.Threading.Tasks;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

namespace Interactables
{
    public class GrillInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform grillPoint;
        [SerializeField] private GameObject cookedSausagePrefab;
        [SerializeField] private GrabbablesComponent grabbablesComponent;
        [SerializeField] private float cookTime = 3f;

        private IInventoryService _inventoryService;

        private bool _isBusy;
        private GameObject _currentCookedSausage;

        public void Initialize(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Interact()
        {
            if (_isBusy)
                return;

            if (_inventoryService.IsInventoryEmpty())
                return;

            var item = _inventoryService.GetGrabbableInInventory();

            if (!item.TryGetComponent<FoodItem>(out var food))
                return;

            if (food.Type != FoodType.RawSausage)
                return;

            _inventoryService.RemoveItem(false);

            item.transform.SetParent(null);


            item.transform.position = grillPoint.position;
            item.transform.rotation = grillPoint.rotation;

            Cook(item).Forget();
        }

        private async UniTaskVoid Cook(Grabbable item)
        {
            _isBusy = true;

            await UniTask.Delay((int)(cookTime * 1000));
            
            if (item != null)
            {
                Object.Destroy(item.gameObject);
            }

            _currentCookedSausage = Object.Instantiate(
                cookedSausagePrefab,
                grillPoint.position,
                grillPoint.rotation);

            var cookedInteractable = _currentCookedSausage.GetComponent<CookedSausageInteractable>();

            if (cookedInteractable != null)
            {
                cookedInteractable.Initialize(_inventoryService, grabbablesComponent);
            }

            _isBusy = false;
        }

        public bool CanInteract() => true;
    }
}