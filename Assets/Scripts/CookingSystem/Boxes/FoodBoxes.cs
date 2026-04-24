using Components;
using Interactables.Interface;
using UnityEngine;

namespace Interactables.Cooking
{
    public class FoodBoxes: MonoBehaviour, IInteractable
    {
        [SerializeField] private Grabbable prefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GrabbablesComponent grabbablesComponent;

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
        {
            return true;
        }
    }
}