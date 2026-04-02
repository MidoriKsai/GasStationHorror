using UnityEngine;
using Core.Interfaces;
using Interactables;
using System.Collections.Generic;

namespace Controllers
{
    public class GrabbablesController : IController
    {
        private Grabbable grabbableInInventory;
        private List<Grabbable> grabbables;

        public Grabbable GetGrabbableInInventory => grabbableInInventory;

        public GrabbablesController(List<Grabbable> grabbables)
        {
            this.grabbables = grabbables;

            SubscribeOnEvents(grabbables);
        }

        public void Drop()
        {
            if (grabbableInInventory != null)
            {
                grabbableInInventory.Drop();
                Debug.Log("Grabbable dropped");
                grabbableInInventory = null;
            }
            else
                Debug.Log("Grabbable slot is empty");
        }

        public void Add(Grabbable grabbable)
        {
            grabbableInInventory = grabbable;
        }

        public void SubscribeOnEvents(List<Grabbable> grabbables)
        {
            foreach(Grabbable grabbable in grabbables)
            {
                grabbable.GrabbedEvent += OnGrabbedEvent;
                grabbable.DroppedEvent += OnDroppedEvent;
            }
        }

        private void OnGrabbedEvent(Grabbable sender, System.EventArgs e)
        {
            grabbableInInventory = sender;
            Debug.Log("YOU GOT GRABBABLE");
        }

        private void OnDroppedEvent(object sender, System.EventArgs e)
        {
            Debug.Log("YOU DROPPED GRABBABLE");
        }

        public void Dispose()
        {

        }
    }
}
