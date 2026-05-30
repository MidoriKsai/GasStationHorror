using Components;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CoffeeMachineInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform machinePoint;
    [SerializeField] private Grabbable coffeePrefab;
    [SerializeField] private GrabbablesComponent grabbablesComponent;
    [SerializeField] private float brewTime = 3f;
    [SerializeField] private AudioClip coffeePourSound;

    private IInventoryService _inventoryService;
    private ISoundService _soundService;

    private bool _isBusy;
    private Grabbable _currentCup;

    public void Initialize(IInventoryService inventoryService, ISoundService soundService)
    {
        _inventoryService = inventoryService;
        _soundService = soundService;
    }

    public void Interact()
    {
        if (_isBusy)
        {
            Debug.Log("Кофемашина занята");
            return;
        }

        if (_inventoryService.IsInventoryEmpty())
        {
            Debug.Log("Нет стакана");
            return;
        }

        var grabbable = _inventoryService.GetGrabbableInInventory();

        if (!grabbable.TryGetComponent<FoodItem>(out var food))
        {
            Debug.Log("Это не предмет для кофе");
            return;
        }

        if (food.Type != FoodType.Cup)
        {
            Debug.Log("Нужен пустой стакан");
            return;
        }

        _inventoryService.RemoveItem(false);

        PlaceCup(grabbable);
        BrewCoffee().Forget();
    }

    private void PlaceCup(Grabbable cup)
    {
        _currentCup = cup;

        cup.transform.SetParent(null);

        cup.transform.position = machinePoint.position;
        cup.transform.rotation = machinePoint.rotation;

        _isBusy = true;
    }

    private async UniTaskVoid BrewCoffee()
    {
        var audioSource = _soundService.Play3DSound(transform.position, coffeePourSound, 0.7f);

        await UniTask.Delay((int)(brewTime * 1000));
        
        if (_currentCup != null)
        {
            Object.Destroy(_currentCup.gameObject);
        }
        
        var coffee = Object.Instantiate(
            coffeePrefab,
            machinePoint.position,
            machinePoint.rotation);

        grabbablesComponent.RegisterNewGrabbable(coffee);

        _isBusy = false;

        audioSource.Stop();

        Debug.Log("Кофе готов");
    }

    public bool CanInteract()
    {
        return true;
    }
}