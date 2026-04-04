using System;
using Interactables.Interface;
using UnityEngine;

public class MoneyView : MonoBehaviour, IInteractable
{
    public event Action Clicked;

    public void Interact()
    {
        Clicked?.Invoke();
    }

    public bool CanInteract()
    {
        return true;
    }
}