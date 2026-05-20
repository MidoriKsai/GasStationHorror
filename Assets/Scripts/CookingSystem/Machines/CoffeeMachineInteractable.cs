using System;
using System.Threading;
using Components;
using Cysharp.Threading.Tasks;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

public class CoffeeMachineInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform machinePoint;
    
    [SerializeField] private GameObject readyCoffeeWithoutLidPrefab;
    
    [SerializeField] private GrabbablesComponent grabbablesComponent;
    [SerializeField] private Animator coffeeMachineAnimator;
    
    [SerializeField] private string pouringBoolParameter = "IsPouring";
    
    [SerializeField] private float brewTime = 3f;
    [SerializeField] private float readyCoffeeFreeDistance = 0.35f;

    private IInventoryService _inventoryService;

    private bool _isBrewing;
    private Grabbable _currentCup;
    private GameObject _readyCoffeeWithoutLid;

    private CancellationTokenSource _destroyCts;

    private void Awake()
    {
        _destroyCts = new CancellationTokenSource();
    }

    public void Initialize(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        Grabbable cup = _inventoryService.GetGrabbableInInventory();

        if (cup == null)
            return;

        _inventoryService.RemoveItem(false);

        PlaceCup(cup);
        BrewCoffee(cup, _destroyCts.Token).Forget();
    }

    private void PlaceCup(Grabbable cup)
    {
        if (cup == null || machinePoint == null)
            return;

        _isBrewing = true;
        _currentCup = cup;

        PrepareItemForMachinePoint(cup);

        cup.transform.SetParent(machinePoint);
        cup.transform.localPosition = Vector3.zero;
        cup.transform.localRotation = Quaternion.identity;
    }

    private async UniTaskVoid BrewCoffee(
        Grabbable cup,
        CancellationToken cancellationToken)
    {
        try
        {
            SetPouringAnimation(true);

            await UniTask.Delay(
                (int)(brewTime * 1000),
                cancellationToken: cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;

            if (this == null)
                return;

            SetPouringAnimation(false);

            if (machinePoint == null)
            {
                ResetBrewingState();
                return;
            }

            Vector3 spawnPosition = machinePoint.position;
            Quaternion spawnRotation = machinePoint.rotation;

            if (cup != null)
            {
                spawnPosition = cup.transform.position;
                spawnRotation = cup.transform.rotation;

                Destroy(cup.gameObject);
            }

            _currentCup = null;
            _isBrewing = false;

            if (readyCoffeeWithoutLidPrefab == null)
            {
                return;
            }

            _readyCoffeeWithoutLid = Instantiate(
                readyCoffeeWithoutLidPrefab,
                spawnPosition,
                spawnRotation);

            ReadyCoffeeInteractable readyCoffeeInteractable =
                _readyCoffeeWithoutLid.GetComponent<ReadyCoffeeInteractable>();

            if (readyCoffeeInteractable != null)
            {
                readyCoffeeInteractable.Initialize(
                    _inventoryService,
                    grabbablesComponent);
            }
            
        }
        catch (OperationCanceledException)
        {
            SetPouringAnimation(false);
        }
    }

    public bool CanInteract()
    {
        RefreshReadyCoffeeState();

        if (_isBrewing)
            return false;

        if (_readyCoffeeWithoutLid != null)
            return false;

        if (_inventoryService == null)
            return false;

        if (_inventoryService.IsInventoryEmpty())
            return false;

        Grabbable grabbable = _inventoryService.GetGrabbableInInventory();

        if (grabbable == null)
            return false;

        return IsCup(grabbable);
    }

    private void RefreshReadyCoffeeState()
    {
        if (_readyCoffeeWithoutLid == null)
        {
            _readyCoffeeWithoutLid = null;
            return;
        }

        if (machinePoint == null)
            return;

        float distance = Vector3.Distance(
            _readyCoffeeWithoutLid.transform.position,
            machinePoint.position);

        if (distance > readyCoffeeFreeDistance)
        {
            _readyCoffeeWithoutLid = null;
        }
    }

    private void PrepareItemForMachinePoint(Grabbable item)
    {
        if (item == null)
            return;

        Rigidbody[] rigidbodies = item.GetComponentsInChildren<Rigidbody>(true);

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].linearVelocity = Vector3.zero;
            rigidbodies[i].angularVelocity = Vector3.zero;
            rigidbodies[i].useGravity = false;
            rigidbodies[i].isKinematic = true;
        }

        Collider[] colliders = item.GetComponentsInChildren<Collider>(true);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    private void SetPouringAnimation(bool value)
    {
        if (coffeeMachineAnimator == null)
            return;

        if (string.IsNullOrEmpty(pouringBoolParameter))
            return;

        coffeeMachineAnimator.SetBool(pouringBoolParameter, value);
    }

    private bool IsCup(Grabbable grabbable)
    {
        if (grabbable == null)
            return false;

        if (grabbable.TryGetComponent(out FoodItem foodItem))
            return foodItem.Type == FoodType.Cup;

        foodItem = grabbable.GetComponentInChildren<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Cup;

        foodItem = grabbable.GetComponentInParent<FoodItem>();

        if (foodItem != null)
            return foodItem.Type == FoodType.Cup;

        return false;
    }

    private void ResetBrewingState()
    {
        SetPouringAnimation(false);

        _isBrewing = false;
        _currentCup = null;
    }

    private void OnDestroy()
    {
        SetPouringAnimation(false);

        if (_destroyCts != null)
        {
            _destroyCts.Cancel();
            _destroyCts.Dispose();
            _destroyCts = null;
        }
    }
}