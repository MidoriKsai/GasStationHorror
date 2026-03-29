using System.Collections.Generic;
using Player;
using UnityEngine;
using Controllers;

namespace Components
{
    public class GrabbablesComponent : MonoBehaviour
    {
        [SerializeField]
        private List<Grabbable> grabbablesList = new();

        [SerializeField]
        private Transform interactablesContainer;

        [SerializeField]
        private Transform grabbablesControllerobj;

        public void Initialize(PlayerDataHandler playerDataHandler)
        {
            foreach (var grabbable in grabbablesList)
            {
                grabbable.Initialize(playerDataHandler.HoldingPoint, interactablesContainer);
            }
        }

        public GrabbablesController CreateController()
        {
            return new GrabbablesController();
        }
    }
}
