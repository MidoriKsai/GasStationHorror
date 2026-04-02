using Core.Interfaces;
using Core;
using UnityEngine;
using Services.Interfaces;

namespace Components
{
    public class PlayerComponent : MonoBehaviour
    {
        private ServiceContainer serviceContainer;
        private IInventoryService inventoryService;

        public IInventoryService GetInventoryService => inventoryService;

        public void SetServiceContainer(ServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            inventoryService = serviceContainer?.Resolve<IInventoryService>();
        }
    }
}
