using UnityEngine;
using Core.Interfaces;
using Interactables;
using System.Collections.Generic;

namespace Controllers
{
    public class GrabbablesController : MonoBehaviour, IController
    {
        private Grabbable grabbableInInventory;

        public Grabbable GrabbableInInventory => grabbableInInventory;

        public void Drop()
        {
            grabbableInInventory.Drop();
            grabbableInInventory = null;
        }

        public void Add(Grabbable grabbable)
        {
            grabbableInInventory = grabbable;
        }

        public void Initializeaaaaaaaa(List<Grabbable> grabbables)
        {
            foreach(Grabbable grabbable in grabbables)
            {
                grabbable.GrabbedEvent += OnGrabbedEvent;
                grabbable.DroppedEvent += OnDroppedEvent;
            }
        }

        private void OnGrabbedEvent(object sender, System.EventArgs e)
        {
            Debug.Log("EVENT HAPPENED");
        }

        private void OnDroppedEvent(object sender, System.EventArgs e)
        {
            Debug.Log("EVENT HAPPENED");
        }

        public void Dispose()
        {

        }
    }
}
