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
    
        [Header("Spawn Points")]
        [SerializeField] private Transform customerSpawnPoint;
        [SerializeField] private Transform carSpawnPoint;
    
        [Header("Paths")]
        [SerializeField] private List<Transform> customerPathPoints;
        [SerializeField] private List<Transform> carArrivePathPoints;
        [SerializeField] private List<Transform> carLeavePathPoints;
    
        private CustomerController _controller;
        
    
        public CustomerController CreateController()
        {
            return new CustomerController(
                customerPrefab,
                carPrefab,
                customerSpawnPoint,
                carSpawnPoint,
                customerPathPoints,
                carArrivePathPoints,
                carLeavePathPoints
            );
        }
    }
}
