using Services.Interfaces;

namespace Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private Grabbable grabbable;

        public void AddItem(Grabbable grabbable)
        {
            this.grabbable = grabbable;
        }

        public void RemoveItem()
        {
            if (!IsInventoryEmpty())
            {
                grabbable.Drop();
            }

            this.grabbable = null;
        }

        public Grabbable GetGrabbableInInventory()
        {
            return grabbable;
        }

        public bool IsInventoryEmpty()
        {
            return grabbable == null;
        }
    }
}