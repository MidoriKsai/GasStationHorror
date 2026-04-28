using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelfSlots : MonoBehaviour
{
    [SerializeField] List<ShelfSlot> shelfSlotsList = new();
    public int RemainingEmptySlots { get; private set; }
    void Start()
    {
        RemainingEmptySlots = shelfSlotsList.Count;
        SubscribeOnFillingEvents();
    }

    void SubscribeOnFillingEvents()
    {
        foreach (ShelfSlot shelfSlot in shelfSlotsList)
        {
            shelfSlot.OnFilledEvent += OnFill;
        }
    }

    void OnFill()
    {
        RemainingEmptySlots--;
    }
}
