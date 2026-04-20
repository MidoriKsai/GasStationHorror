using Core.Interfaces;
using Cysharp.Threading.Tasks;
using NPCSystem;
using UnityEngine;

public class CustomerController : IController
{
    private readonly CustomerSpawner _customerSpawner;
    private readonly CarSpawner _carSpawner;
    private readonly CustomersFactory _customersFactory;

    private readonly PointsHandler pointsHandler;

    private Customer _currentCustomer;
    private Car _currentCar;
    private CustomerData _currentCustomerData;

    private UniTaskCompletionSource _carTcs;
    private UniTaskCompletionSource _customerTcs;
    private Transform _gasStationPoint;

    public CustomerData currentCustomerData => _currentCustomerData;
    
    public CustomerController(
        PointsHandler pointsHandler,
        Customer customerPrefab,
        Car carPrefab)
    {
        _customerSpawner = new CustomerSpawner(customerPrefab);
        _carSpawner = new CarSpawner(carPrefab);
        _customersFactory = new CustomersFactory();

        this.pointsHandler = pointsHandler;
    }

    public Transform GetCustomerDialogPoint()
    {
        if (_currentCustomer != null)
            return _currentCustomer.GetDialogPoint();

        return null;
    }

    public async UniTask WaitForCustomerArriveAsync()
    {
        _currentCustomerData = _customersFactory.CreateCustomer();

        int petrolPumpNumber = _currentCustomerData.petrolPumpNumber;

        _gasStationPoint = pointsHandler.GasStationPoints[petrolPumpNumber];
        
        Debug.Log("Колонка" + petrolPumpNumber);

        _currentCar = _carSpawner.Spawn(pointsHandler.CarSpawnPoint);

        await WaitForCarPathAsync(_gasStationPoint);

        _currentCustomer = _customerSpawner.Spawn(_currentCar.GetCustomerSpawnPoint(), _currentCustomerData);

        await WaitForCustomerPathAsync(pointsHandler.CashDeskPoint);
    }

    public async UniTask WaitForCustomerLeaveAsync()
    {
        await WaitForCustomerPathAsync(_currentCar.GetCustomerSpawnPoint());

        if (_currentCustomer != null)
        {
            Object.Destroy(_currentCustomer.gameObject);
            _currentCustomer = null;
        }

        await WaitForCarPathAsync(pointsHandler.CarLeavePoint);

        if (_currentCar != null)
        {
            Object.Destroy(_currentCar.gameObject);
            _currentCar = null;
        }

        _currentCustomerData = null;
    }

    private async UniTask WaitForCarPathAsync(Transform targetPoint)
    {
        _carTcs = new UniTaskCompletionSource();
        _currentCar.StartPath(new AgentPath(targetPoint), () => OnPathFinished(_carTcs));
        await _carTcs.Task;
    }

    private async UniTask WaitForCustomerPathAsync(Transform targetPoint)
    {
        _customerTcs = new UniTaskCompletionSource();
        _currentCustomer.StartPath(new AgentPath(targetPoint), () => OnPathFinished(_customerTcs));
        await _customerTcs.Task;
    }

    private void OnPathFinished(UniTaskCompletionSource tcs)
    {
        tcs?.TrySetResult();
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

        _currentCustomerData = null;
    }
}