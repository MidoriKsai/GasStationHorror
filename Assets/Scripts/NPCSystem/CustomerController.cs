using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

public class CustomerController : IController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly List<Transform> _spawnPoints;
    private readonly CustomerPath _customerPath;
    private Customer _currentCustomer;
    private bool _isMoving;

    public CustomerController(Customer customerPrefab, List<Transform> spawnPoint, List<Transform> pathPoints)
    {
        _spawnPoints = spawnPoint;
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _customerPath = new CustomerPath(pathPoints.ToArray());

        OnGameStart();
    }

    private void OnGameStart()
    {
        var customer = _customerSpawner.SpawnOnRandom(_spawnPoints.ToArray());
        customer.StartPath(_customerPath);
    }

    public void Dispose()
    {
    }
}