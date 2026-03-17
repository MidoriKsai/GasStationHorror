using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Components
{
    public class CustomersComponent : MonoBehaviour, IComponent<CustomerController>
    {
        [SerializeField] private Customer customerPrefab;
        [SerializeField] private List<Transform> spawnPoint;
        [SerializeField] private List<Transform> pathPoints;

        private CustomerController customerController;

        private int customerId;

        public CustomerController CreateController()
        {
            return new CustomerController(customerPrefab, spawnPoint, pathPoints);
        }
    }
}