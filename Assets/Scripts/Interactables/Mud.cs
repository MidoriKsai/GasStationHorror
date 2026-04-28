using UnityEngine;
using System.Collections;
using Interactables.Interface;
using System;

public class Mud : MonoBehaviour, IInteractable
{
    public float WipePercentage => wipeProgress / timeToWipe * 100f;
    public event Action<Mud> WashedAction;

    private float timeToWipe = 3f;
    private float wipeProgress;

    public void Interact()
    {
        wipeProgress += Time.deltaTime;

        if (wipeProgress >= timeToWipe)
        {
            gameObject.SetActive(false);
            WashedAction?.Invoke(this);
        }
    }

    public void DestroyMud()
    {
        Destroy(gameObject);
    }

    public bool CanInteract() => true;
}
