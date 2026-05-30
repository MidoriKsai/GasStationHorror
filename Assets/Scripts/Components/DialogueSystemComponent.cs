using Controllers;
using Core;
using Core.Interfaces;
using DialogueSystem;
using DialogueSystem.Interfaces;
using Services.Interfaces;
using UnityEngine;

namespace Components
{
    public class DialogueSystemComponent : MonoBehaviour, IComponent<DialogueSystemController>
    {
        [SerializeField]
        private DialogueView _dialogueView;

        [SerializeField]
        private PlayerControls _playerMovement;

        private ServiceContainer _serviceContainer;

        public void Initialize(ServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        public DialogueSystemController CreateController()
        {
            var view = (IDialogueView)_dialogueView;

            var xmlParser = _serviceContainer.Resolve<IXMLParserService>();

            var parser = new Parser(xmlParser);

            var storage = new DialogueStorage(parser);

            return new DialogueSystemController(
                view,
                storage,
                _playerMovement);
        }
    }
}