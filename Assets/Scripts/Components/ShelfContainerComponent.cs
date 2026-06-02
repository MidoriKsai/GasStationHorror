using System.Collections.Generic;
using Components;
using Services.Interfaces;
using UnityEngine;
using Interactables;
using System.Threading;
using TipsSystem;

public class ShelfContainerComponent : MonoBehaviour
{
    [SerializeField] private List<Shelf> shelfsContainer = new();
    public int RemainingShelfs { get; private set; }

    public void SetupShelfs(CancellationToken ct)
    {
        foreach (Shelf shelf in shelfsContainer)
        {
            shelf.EmptyShelf();
            shelf.SetCancellationToken(ct);
        }
    }

    public void InitializeShelfs(IPlayerService playerService, IInventoryService inventoryService, TipController tipController)
    {
        RemainingShelfs = shelfsContainer.Count;

        foreach (Shelf shelf in shelfsContainer)
        {
            shelf.Initialize(
                inventoryService,
                playerService,
                tipController
            );
            shelf.shelfFilledEvent += OnShelfFilled;
        }
    }

    private void OnShelfFilled()
    {
        RemainingShelfs--;
    }
}
