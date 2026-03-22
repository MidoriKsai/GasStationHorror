using System.Collections.Generic;
using Core.Interfaces;
using NPCSystem;
using UnityEngine;

public class CustomerController : IController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly CarSpawner _carSpawner;

    private readonly Transform _customerSpawnPoint;
    private readonly Transform _carSpawnPoint;

    private readonly CustomerPath _customerPath;
    private readonly CarPath _carArrivePath;
    private readonly CarPath _carLeavePath;

    private Customer _currentCustomer;
    private Car _currentCar;

    public CustomerController(
        Customer customerPrefab,
        Car carPrefab,
        Transform customerSpawnPoint,
        Transform carSpawnPoint,
        List<Transform> customerPathPoints,
        List<Transform> carArrivePathPoints,
        List<Transform> carLeavePathPoints)
    {
        Debug.Log("CustomerController created");
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _carSpawner = new CarSpawner(carPrefab);

        _customerSpawnPoint = customerSpawnPoint;
        _carSpawnPoint = carSpawnPoint;

        _customerPath = new CustomerPath(customerPathPoints.ToArray());
        _carArrivePath = new CarPath(carArrivePathPoints.ToArray());
        _carLeavePath = new CarPath(carLeavePathPoints.ToArray());

        OnGameStart();
    }

    private void OnGameStart()
    {
        SpawnCarAndStartArrival();
    }

    private void SpawnCarAndStartArrival()
    {
        _currentCar = _carSpawner.Spawn(_carSpawnPoint);
        _currentCar.StartPath(_carArrivePath, OnCarArrived);
    }

    private void OnCarArrived()
    {
        _currentCustomer = _customerSpawner.Spawn(_customerSpawnPoint);
        _currentCustomer.StartPath(_customerPath, OnCustomerFinished);
    }

    private void OnCustomerFinished()
    {
        if (_currentCustomer != null)
            Object.Destroy(_currentCustomer.gameObject);
        
        _currentCar.StartPath(_carLeavePath, OnCarLeft);
    }

    private void OnCarLeft()
    {
        

        if (_currentCar != null)
            Object.Destroy(_currentCar.gameObject);
    }

    public void Dispose()
    {
        if (_currentCustomer != null)
            Object.Destroy(_currentCustomer.gameObject);

        if (_currentCar != null)
            Object.Destroy(_currentCar.gameObject);
    }
}