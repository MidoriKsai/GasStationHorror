using UnityEngine;

public class CustomerSpawner
{
    
    private Customer _customerPrefab;
    
    private CustomersFactory _factory;
    
    public CustomerSpawner(Customer customerPrefab)
    {
        _customerPrefab = customerPrefab;
        _factory = new CustomersFactory();
    }
    
    public void Spawn(Transform spawnPoint)
    {
        var customerData = _factory.CreateCustomer();
        var customer = Object.Instantiate(_customerPrefab, spawnPoint.position, spawnPoint.rotation);
        customer.Init(customerData);
        Debug.Log("Spawn Customer");
    }

    public void SpawnOnRandom(Transform[] spawnPoints)
    {
        var random = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[random];
        Spawn(spawnPoint);
    }

}
