using System.Collections.Generic;
using Player;
using UnityEngine;
using Controllers;
using Services.Interfaces;
using Core;
using Core.Interfaces;

namespace Components
{
    public class GrabbablesComponent : MonoBehaviour, IComponent<GrabbablesController>
    {
        [SerializeField]
        private List<Grabbable> grabbablesList = new();

        [SerializeField]
        private Transform interactablesContainer;

        // [SerializeField]
        // private Transform grabbablesControllerОbject;

        private IPlayerService playerService;

        public void Initialize(ServiceContainer serviceContainer)
        {
            playerService = serviceContainer.Resolve<IPlayerService>();

            foreach (var grabbable in grabbablesList)
            {
                Transform holdingPoint = playerService.GetGrabbablesHoldingPoint();
                grabbable.Initialize(holdingPoint, interactablesContainer);
                grabbable.transform.SetParent(interactablesContainer);
            }
        }

        public GrabbablesController CreateController()
        {
            return new GrabbablesController(grabbablesList);
        }
    }
}
