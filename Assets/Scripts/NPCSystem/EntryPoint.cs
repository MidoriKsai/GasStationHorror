using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform[] spawnPoint;
    
    private CustomerController customerController;
    
    private int customerId;

    private void Start()
    {
        customerController = new CustomerController(customerPrefab, spawnPoint);

    }

}
