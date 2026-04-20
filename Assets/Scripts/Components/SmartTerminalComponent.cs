using Core;
using Core.Interfaces;
using Interactables.SmartTerminal;
using UnityEngine;

namespace Components
{
    public class SmartTerminalComponent : MonoBehaviour, IComponent<SmartTerminalController>
    {
        [SerializeField] private SmartTerminalView smartTerminalView;
        [SerializeField] private SmartTerminalInteractable smartTerminalInteractable;

        private ServiceContainer _container;
        private SmartTerminalController _controller;

        public void Initialize(ServiceContainer container)
        {
            _container = container;
        }

        public SmartTerminalController CreateController()
        {
            _controller = new SmartTerminalController(
                smartTerminalView,
                smartTerminalInteractable);

            _controller.InitializeInteract();

            return _controller;
        }
    }
}