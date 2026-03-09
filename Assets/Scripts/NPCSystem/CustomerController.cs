using UnityEngine;

public class CustomerController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly Transform[] _spawnPoints;
    
    public CustomerController(Customer customerPrefab, Transform[] spawnPoint)
    {
        _spawnPoints = spawnPoint;
        _customerSpawner = new CustomerSpawner(customerPrefab);
        OnGameStart();
    }

    private void OnGameStart()
    {
        _customerSpawner.SpawnOnRandom(_spawnPoints);
        
    }

}