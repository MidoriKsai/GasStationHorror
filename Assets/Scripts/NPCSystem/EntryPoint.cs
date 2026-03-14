using UnityEngine;
using UnityEngine.AI;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Transform[] pathPoints;
    
    private CustomerController customerController;
    
    private int customerId;

    private void Start()
    {
        customerController = new CustomerController(customerPrefab, spawnPoint, pathPoints);

    }

}
