using Core;
using Core.Interfaces;
using Interactables.SmartTerminal;
using Services.Interfaces;
using UnityEngine;

namespace Components
{
    public class SmartTerminalComponent : MonoBehaviour, IComponent<SmartTerminalController>
    {
        [SerializeField] private SmartTerminalView smartTerminalView;
        [SerializeField] private SmartTerminalInteractable smartTerminalInteractable;
        [SerializeField] private Transform terminalFocusPoint;

        private ServiceContainer _container;
        private SmartTerminalController _controller;
        private TipsSystem.TipController _tipController;

        public void Initialize(ServiceContainer container)
        {
            _container = container;
        }

        public SmartTerminalController CreateController()
        {
            var playerService = _container.Resolve<IPlayerService>();

            _controller = new SmartTerminalController(
                smartTerminalView,
                smartTerminalInteractable,
                playerService,
                terminalFocusPoint,
                _tipController);

            _controller.InitializeInteract();

            return _controller;
        }
    }
}