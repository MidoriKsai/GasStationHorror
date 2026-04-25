using UnityEngine;

public class CustomerSpawner
{
    private Customer _customerPrefab;

    public CustomerSpawner(Customer customerPrefab)
    {
        _customerPrefab = customerPrefab;
    }

    public Customer Spawn(Transform spawnPoint, CustomerData customerData)
    {
        var customer = Object.Instantiate(_customerPrefab, spawnPoint.position, spawnPoint.rotation);
        customer.Init(customerData);
        Debug.Log("Spawn Customer");
        return customer;
    }

    public Customer SpawnOnRandom(Transform[] spawnPoints, CustomerData customerData)
    {
        var random = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[random];
        return Spawn(spawnPoint, customerData);
    }
}