using Player;
using Services.Interfaces;
using UnityEngine;
using Utils;
using Controllers;

namespace Services.Implementations
{
    public class InventoryService : IInventoryService
    {

        GrabbablesController grabbablesController;

        public void Initialize(GrabbablesController grabbablesController)
        {
            this.grabbablesController = grabbablesController;
        }

        public void AddItem(Grabbable grabbable)
        {
            grabbablesController.Add(grabbable);
        }

        public void RemoveItem()
        {
            grabbablesController.Drop();
        }

        public Grabbable GetGrabbableInInventory()
        {
            return grabbablesController.GrabbableInInventory;
        }
    }
}
