using Core;
using Services.Interfaces;
using TipsSystem;
using UnityEngine;

namespace Components
{
    public class InteractablesTipsComponent : MonoBehaviour
    {
        [SerializeField] private UrnInteractable urnInteractable;

        [SerializeField] private CashRegisterInteractable cashRegisterInteractable;

        public void Initialize(
            ServiceContainer serviceContainer,
            TipController tipController)
        {
            IInventoryService inventoryService =
                serviceContainer.Resolve<IInventoryService>();

            if (urnInteractable != null)
            {
                urnInteractable.Initialize(
                    inventoryService,
                    tipController);
            }

            if (cashRegisterInteractable != null)
            {
                cashRegisterInteractable.Initialize(
                    inventoryService);
            }
        }
    }
}