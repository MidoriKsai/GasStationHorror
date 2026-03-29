using UnityEngine;
using Services.Interfaces;
using Services.Implementations;
using Core;


namespace Player
{
    class PlayerInventoryController : MonoBehaviour
    {
        IInventoryService inventoryService;

        void Start()
        {
            // inventoryService = serviceContainer.Resolve<IInventoryService>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                inventoryService.RemoveItem();
                Debug.Log("YOU'VE TRIED TO DROP ITEM");
            }
        }


    }
}
