using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Components
{
    public class GrabbablesComponent : MonoBehaviour
    {
        [SerializeField]
        private List<Grabbable> grabbablesList = new();

        [SerializeField]
        private Transform interactablesContainer;

        public void Initialize(PlayerDataHandler playerDataHandler)
        {
            foreach (var grabbable in grabbablesList)
            {
                grabbable.Initialize(playerDataHandler.HoldingPoint, interactablesContainer);
            }
        }
    }
}