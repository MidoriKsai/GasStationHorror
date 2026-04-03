using UnityEngine;
using Interactables;
using Controllers;

namespace Services.Interfaces
{
    public interface IInventoryService
    {
        void AddItem(Grabbable grabbable);
        void RemoveItem();
        Grabbable GetGrabbableInInventory();
        bool IsInventoryEmpty();
    }
}