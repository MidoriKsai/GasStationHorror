using UnityEngine;
using System.Collections;
using Interactables.Interface;
using System;

public class Mud : MonoBehaviour, IInteractable
{
    public float WipePercentage => wipeProgress / timeToWipe * 100f;
    public event Action<Mud> WashedAction;

    [SerializeField] private float timeToWipe = 3f;
    private float wipeProgress;
    
    [SerializeField] private float scrubDistance = 0.15f;
    [SerializeField] private float scrubSpeed = 5f;
    
    private GameObject mop;
    private Vector3 mopInitialLocalPosition;
    private bool hasSavedInitialPosition;

    public void Interact()
    {
        if (mop != null && !hasSavedInitialPosition)
        {
            mopInitialLocalPosition = mop.transform.localPosition;
            hasSavedInitialPosition = true;
        }

        wipeProgress += Time.deltaTime;

        if (mop != null)
        {
            AnimateMop();
        }

        if (wipeProgress >= timeToWipe)
        {
            ResetMopPosition();
            gameObject.SetActive(false);
            WashedAction?.Invoke(this);
        }
    }

    private void AnimateMop()
    {
        float lerpValue = Mathf.PingPong(wipeProgress * scrubSpeed, 1f);


        Vector3 forwardDirection = transform.forward * scrubDistance; 

        Vector3 startPos = mopInitialLocalPosition + forwardDirection;
        Vector3 endPos = mopInitialLocalPosition - forwardDirection;

        mop.transform.localPosition = Vector3.Lerp(startPos, endPos, lerpValue);
    }

    private void ResetMopPosition()
    {
        if (mop != null && hasSavedInitialPosition)
        {
            mop.transform.localPosition = mopInitialLocalPosition;
            hasSavedInitialPosition = false;
        }
    }

    public void DestroyMud()
    {
        Destroy(gameObject);
    }

    public void SetMop(GameObject mop)
    {
        if (mop == null)
            return;

        this.mop = mop;
    }

    public bool CanInteract() => true;

    private void OnDisable()
    {
        ResetMopPosition();
    }
}
