using UnityEngine;

public class CustomerController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly Transform[] _spawnPoints;
    private readonly CustomerPath _customerPath;
    private Customer _currentCustomer;
    private bool _isMoving;
    
    
    public CustomerController(Customer customerPrefab, Transform[] spawnPoint, Transform[] pathPoints)
    {
        _spawnPoints = spawnPoint;
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _customerPath = new CustomerPath(pathPoints);
        
        OnGameStart();
    }

    private void OnGameStart()
    { 
        var customer = _customerSpawner.SpawnOnRandom(_spawnPoints);
        customer.StartPath(_customerPath);
        
    }
    

}