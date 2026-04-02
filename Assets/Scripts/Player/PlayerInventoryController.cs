using UnityEngine;
using Services.Interfaces;
using Services.Implementations;
using Core;
using Components;

namespace Player
{
    class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField]
        private PlayerComponent playerComponent;
        private IInventoryService inventoryService;

        void Start()
        {
            inventoryService = playerComponent.GetInventoryService;
            if (inventoryService == null)
                Debug.Log("ERROR: inventoryService is not found!");
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
