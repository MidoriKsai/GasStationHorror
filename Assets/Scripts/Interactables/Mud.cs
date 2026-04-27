using UnityEngine;
using System.Collections;
using Interactables.Interface;

public class Mud : MonoBehaviour, IInteractable
{
    public float WipePercentage => wipeProgress / timeToWipe * 100f;

    [SerializeField] private int NeededWipeCount;
    private float timeToWipe = 3f;
    private float wipeProgress;

    public void Interact()
    {
        wipeProgress += Time.deltaTime;

        Debug.Log($"WIPING {wipeProgress}");

        if (wipeProgress >= timeToWipe)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public bool CanInteract() => true;
}
