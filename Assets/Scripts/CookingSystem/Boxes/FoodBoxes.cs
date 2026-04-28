using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

namespace Interactables.Cooking
{
    public class FoodBoxes: MonoBehaviour, IInteractable
    {
        [SerializeField] private Grabbable prefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GrabbablesComponent grabbablesComponent;
        private IInventoryService inventoryService;

        void Start()
        {
            inventoryService = grabbablesComponent.GetInventoryService();
        }

        public void Interact()
        {
            if (prefab == null)
            {
                return;
            }

            var food = Object.Instantiate(
                prefab,
                spawnPoint.position,
                spawnPoint.rotation);
            
            grabbablesComponent.RegisterNewGrabbable(food);

            food.TryGrab();
        }

        public bool CanInteract()
        => inventoryService.IsInventoryEmpty();
    }
}