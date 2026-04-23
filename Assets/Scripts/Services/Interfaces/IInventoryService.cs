using UnityEngine;
using Interactables;
using Controllers;

namespace Services.Interfaces
{
    public interface IInventoryService
    {
        void AddItem(Grabbable grabbable);
        void RemoveItem(bool needToAddForce);
        Grabbable GetGrabbableInInventory();
        bool IsInventoryEmpty();
    }
}