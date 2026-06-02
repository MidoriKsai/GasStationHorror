using UnityEngine;
using Interactables.Interface;
using Services.Interfaces;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TipsSystem;

public class Shelf : MonoBehaviour, IInteractable
{
    public event Action shelfFilledEvent;

    [SerializeField] private GameObject filledShelf;
    [SerializeField] private GameObject emptyShelf;
    [SerializeField] private AudioSource audioSource;

    private IPlayerService playerService;
    private IInventoryService inventoryService;
    private TipController tipController;
    private CancellationToken ct;

    private bool isShelfFilled;

    public void Initialize(IInventoryService inventoryService, IPlayerService playerService, TipController tipController)
    {
        this.inventoryService = inventoryService;
        this.playerService = playerService;
        this.tipController = tipController;
    }

    public void SetCancellationToken(CancellationToken ct)
    {
        this.ct = ct;
    }

    public bool CanInteract()
    {
        return !isShelfFilled;
    }

    public void Interact()
    {
        var grabbable = inventoryService.GetGrabbableInInventory();

        if (inventoryService.IsInventoryEmpty())
        {
            tipController?.ShowPopup("shelf_need_box");
            return;
        }

        if (!grabbable.CompareTag("ShelfBox"))
        {
            tipController?.ShowPopup("shelf_wrong_item");
            return;
        }

        FillShelf().Forget();

        inventoryService.RemoveItem(false);
        Destroy(grabbable.gameObject);
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
}