using UnityEngine;
using Player;
using Interactables.Interface;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private PlayerInteractionController playerInteractionController;
    private RectTransform rectTransform;
    private float defaultWidth;


    void Start()
    {
        playerInteractionController.InteractionEvent += UpdateProgressBar;

        rectTransform = GetComponent<RectTransform>();
        defaultWidth = rectTransform.rect.width;
    }

    void Update()
    {
        if (playerInteractionController.CurrentInteractable == null)
            HideProgressBar();
    }

    void UpdateProgressBar(IInteractable interactable)
    {
        if (interactable is Mud)
            ShowProgressBarPercentage(((Mud)interactable).WipePercentage);
        else
            HideProgressBar();
    }

    void ShowProgressBarPercentage(float percentage)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percentage / 100f * defaultWidth);
        ShowProgressBar();
    }

    void ShowProgressBar()
    {
        gameObject.SetActive(true);
    }

    void HideProgressBar()
    {
        gameObject.SetActive(false);
    }
}
