using System;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    public event Action Clicked;

    private void OnMouseDown()
    {
        Clicked?.Invoke();
    }
}