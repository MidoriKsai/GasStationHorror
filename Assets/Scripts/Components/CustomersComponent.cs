using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Components
{
    public class CustomersComponent : MonoBehaviour, IComponent<CustomerController>
    {
        [Header("Prefabs")]
        [SerializeField] private Customer customerPrefab;
        [SerializeField] private NPCSystem.Car carPrefab;

        private CustomerController _controller;
        private PointsHandler pointsHandler;

        public void Initialize(PointsHandler pointsHandler)
        {
            this.pointsHandler = pointsHandler;
        }

        public CustomerController CreateController()
        {
            return new CustomerController(
                pointsHandler,
                customerPrefab,
                carPrefab
            );
        }
    }
}