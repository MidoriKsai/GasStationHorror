using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
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

    private UniTaskCompletionSource _carArrivedTcs;
    private UniTaskCompletionSource _customerArrivedTcs;
    private UniTaskCompletionSource _carLeftTcs;

    public CustomerController(
        Customer customerPrefab,
        Car carPrefab,
        Transform customerSpawnPoint,
        Transform carSpawnPoint,
        List<Transform> customerPathPoints,
        List<Transform> carArrivePathPoints,
        List<Transform> carLeavePathPoints)
    {
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _carSpawner = new CarSpawner(carPrefab);

        _customerSpawnPoint = customerSpawnPoint;
        _carSpawnPoint = carSpawnPoint;

        _customerPath = new CustomerPath(customerPathPoints.ToArray());
        _carArrivePath = new CarPath(carArrivePathPoints.ToArray());
        _carLeavePath = new CarPath(carLeavePathPoints.ToArray());
    }

    public async UniTask SpawnCustomerSequenceAsync()
    {
        await SpawnCarAsync();
        await SpawnCustomerAsync();
    }

    private async UniTask SpawnCarAsync()
    {
        _carArrivedTcs = new UniTaskCompletionSource();

        _currentCar = _carSpawner.Spawn(_carSpawnPoint);
        _currentCar.StartPath(_carArrivePath, OnCarArrived);

        await _carArrivedTcs.Task;
    }

    private void OnCarArrived()
    {
        _carArrivedTcs?.TrySetResult();
    }

    private async UniTask SpawnCustomerAsync()
    {
        _customerArrivedTcs = new UniTaskCompletionSource();

        _currentCustomer = _customerSpawner.Spawn(_customerSpawnPoint);
        _currentCustomer.StartPath(_customerPath, OnCustomerArrived);

        await _customerArrivedTcs.Task;
    }

    private void OnCustomerArrived()
    {
        _customerArrivedTcs?.TrySetResult();
    }

    public async UniTask CustomerLeaveAsync()
    {
        _carLeftTcs = new UniTaskCompletionSource();

        if (_currentCustomer != null)
        {
            Object.Destroy(_currentCustomer.gameObject);
            _currentCustomer = null;
        }

        if (_currentCar != null)
        {
            _currentCar.StartPath(_carLeavePath, OnCarLeft);
            await _carLeftTcs.Task;
        }
    }

    private void OnCarLeft()
    {
        if (_currentCar != null)
        {
            Object.Destroy(_currentCar.gameObject);
            _currentCar = null;
        }

        _carLeftTcs?.TrySetResult();
    }

    public Customer GetCurrentCustomer()
    {
        return _currentCustomer;
    }

    public void Dispose()
    {
        if (_currentCustomer != null)
        {
            Object.Destroy(_currentCustomer.gameObject);
            _currentCustomer = null;
        }

        if (_currentCar != null)
        {
            Object.Destroy(_currentCar.gameObject);
            _currentCar = null;
        }
    }
}