using System.Collections.Generic;
using Components;
using Unity.VisualScripting;
using UnityEngine;

public class ShelfSlots : MonoBehaviour
{
    [SerializeField] private List<ShelfSlot> shelfSlotsList = new();
    [SerializeField] private GrabbablesComponent grabbablesComponent;
    public int RemainingEmptySlots { get; private set; }
    void Start()
    {
        RemainingEmptySlots = shelfSlotsList.Count;
        SubscribeOnFillingEvents();
        SetGrabbablesComponentInShelfs(grabbablesComponent);
    }

    void SubscribeOnFillingEvents()
    {
        foreach (ShelfSlot shelfSlot in shelfSlotsList)
        {
            shelfSlot.OnFilledEvent += OnFill;
        }
    }

    void SetGrabbablesComponentInShelfs(GrabbablesComponent grabbablesComponent)
    {
        foreach (var slot in shelfSlotsList)
        {
            slot.SetGrabbablesComponent(grabbablesComponent);
        }
    }

    void OnFill()
    {
        RemainingEmptySlots--;
    }
}
