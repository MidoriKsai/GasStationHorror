using Services.Implementations;
using Services.Interfaces;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1)]
    public abstract class BaseContext : MonoBehaviour
    {
        private static ServiceContainer serviceContainer;

        private void Awake()
        {
            if (serviceContainer == null)
            {
                serviceContainer = new ServiceContainer();
                InitializeServices(serviceContainer);
            }

            Initialize(serviceContainer);
        }

        private void OnDestroy()
        {
            Deinitialize();
        }

        protected abstract void Initialize(ServiceContainer serviceContainer);

        protected abstract void Deinitialize();

        private void InitializeServices(ServiceContainer serviceContainer)
        {
            serviceContainer.Register<IPlayerService>(new PlayerService());
            serviceContainer.Register<IXMLParserService>(new XMLParserService());
            serviceContainer.Register<IInventoryService>(new InventoryService());
        }
    }
}
