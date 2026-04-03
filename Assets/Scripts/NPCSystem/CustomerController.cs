using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using NPCSystem;
using UnityEngine;

public class CustomerController : IController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly CarSpawner _carSpawner;

    private readonly PointsHandler pointsHandler;

    private Customer _currentCustomer;
    private Car _currentCar;

    private UniTaskCompletionSource _carArrivedTcs;
    private UniTaskCompletionSource _customerArrivedTcs;
    private UniTaskCompletionSource _carLeftTcs;

    public CustomerController(
        PointsHandler pointsHandler,
        Customer customerPrefab,
        Car carPrefab)
    {
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _carSpawner = new CarSpawner(carPrefab);

        this.pointsHandler = pointsHandler;
    }

    public async UniTask WaitForCustomerArriveAsync()
    {
        await SpawnCarAsync();
        await SpawnCustomerAsync();
    }

    private async UniTask SpawnCarAsync()
    {
        _carArrivedTcs = new UniTaskCompletionSource();

        _currentCar = _carSpawner.Spawn(pointsHandler.CarSpawnPoint);
        _currentCar.StartPath(new AgentPath(pointsHandler.GasStationPoint), OnCarArrived);

        await _carArrivedTcs.Task;
    }

    private void OnCarArrived()
    {
        _carArrivedTcs?.TrySetResult();
    }

    private async UniTask SpawnCustomerAsync()
    {
        _customerArrivedTcs = new UniTaskCompletionSource();

        _currentCustomer = _customerSpawner.Spawn(_currentCar.GetCustomerSpawnPoint());
        _currentCustomer.StartPath(new AgentPath(pointsHandler.CashDeskPoint), OnCustomerArrived);

        await _customerArrivedTcs.Task;
    }

    private void OnCustomerArrived()
    {
        _customerArrivedTcs?.TrySetResult();
    }

    public async UniTask CustomerLeaveAsync()
    {
        _carLeftTcs = new UniTaskCompletionSource();

        // TODO: Customer go to car.
        if (_currentCustomer != null)
        {
            Object.Destroy(_currentCustomer.gameObject);
            _currentCustomer = null;
        }

        if (_currentCar != null)
        {
            _currentCar.StartPath(new AgentPath(pointsHandler.CarLeavePoint), OnCarLeft);
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