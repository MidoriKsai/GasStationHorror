//using UnityEngine;
//using Interactables.Interface;
//using Components;
//using Services.Interfaces;
//using System;
//using Services.Implementations;
//using UnityEngine.Playables;
//using System.Threading;
//
//public class Shelf : MonoBehaviour, IInteractable
//{
//    public event Action shelfFilledEvent;
//    [SerializeField] private GameObject filledShelf;
//    [SerializeField] private GameObject emptyShelf;
//    private IPlayerService playerService;
//    private bool isShelfFilled;
//    private IInventoryService inventoryService;
//    private CancellationToken ct;
//    public bool CanInteract()
//        => !inventoryService.IsInventoryEmpty()
//            && inventoryService.GetGrabbableInInventory().CompareTag("ShelfBox");
//
//    public void Interact()
//    {
//        if (!isShelfFilled)
//        {
//            FillShelf();
//            Grabbable shelfBox = inventoryService.GetGrabbableInInventory();
//            inventoryService.RemoveItem(needToAddForce: false);
//            Destroy(shelfBox.gameObject);
//        }
//    }
//
//    private void FillShelf()
//    {
//        playerService.FadeInAsync(ct);
//        filledShelf.SetActive(true);
//        emptyShelf.SetActive(false);
//        playerService.FadeOutAsync(ct);
//        isShelfFilled = true;
//        shelfFilledEvent?.Invoke();
//    }
//
//    public void EmptyShelf()
//    {
//        filledShelf.SetActive(false);
//        emptyShelf.SetActive(true);
//        isShelfFilled = false;
//    }
//
//    public void Initialize(IInventoryService inventoryService, IPlayerService playerService)
//    {
//        this.inventoryService = inventoryService;
//        this.playerService = playerService;
//    }
//
//    public void SetCancellationToken(CancellationToken ct)
//    {
//        this.ct = ct;
//    }
//
//
//}
//
//
using UnityEngine;
using Interactables.Interface;
using Services.Interfaces;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Shelf : MonoBehaviour, IInteractable
{
    public event Action shelfFilledEvent;

    [SerializeField] private GameObject filledShelf;
    [SerializeField] private GameObject emptyShelf;
    [SerializeField] private AudioSource audioSource;

    private IPlayerService playerService;
    private bool isShelfFilled;
    private IInventoryService inventoryService;
    private CancellationToken ct;

    public bool CanInteract()
        => !inventoryService.IsInventoryEmpty() &&
            inventoryService.GetGrabbableInInventory().CompareTag("ShelfBox");

    public void Interact()
    {
        if (!isShelfFilled)
        {
            FillShelf().Forget();
            Grabbable shelfBox = inventoryService.GetGrabbableInInventory();
            inventoryService.RemoveItem(needToAddForce: false);
            Destroy(shelfBox.gameObject);
        }
    }

    private async UniTask FillShelf()
    {
        await playerService.FadeInAsync(ct);
        audioSource.Play();
        filledShelf.SetActive(true);
        emptyShelf.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        audioSource.Stop();
        await playerService.FadeOutAsync(ct);
        isShelfFilled = true;
        shelfFilledEvent?.Invoke();
    }

    public void EmptyShelf()
    {
        filledShelf.SetActive(false);
        emptyShelf.SetActive(true);
        isShelfFilled = false;
    }

    public void Initialize(IInventoryService inventoryService, IPlayerService playerService)
    {
        this.inventoryService = inventoryService;
        this.playerService = playerService;
    }

    public void SetCancellationToken(CancellationToken ct)
    {
        this.ct = ct;
    }
}