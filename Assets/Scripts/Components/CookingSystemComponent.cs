using Core;
using Interactables;
using Interactables.Cooking;
using Services.Interfaces;
using UnityEngine;

namespace Components
{
    public class CookingSystemComponent : MonoBehaviour
    {
        [Header("Boxes")]
        [SerializeField] private FoodBoxes[] foodBoxes;

        [Header("Stations")]
        [SerializeField] private GrillInteractable grill;
        [SerializeField] private CoffeeMachineInteractable coffeeMachine;

        public void Initialize(ServiceContainer container)
        {
            var inventory = container.Resolve<IInventoryService>();
            var soundsSystem = container.Resolve<ISoundService>();

            grill.Initialize(inventory, soundsSystem);
            coffeeMachine.Initialize(inventory, soundsSystem);
        }
    }
}