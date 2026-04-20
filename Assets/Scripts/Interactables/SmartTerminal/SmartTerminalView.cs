using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SmartTerminalView : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Panels")]
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject receiptPanel;


    [Header("Input Panel")]
    [SerializeField] private TMP_InputField pumpNumber;
    [SerializeField] private TMP_InputField litersCount;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Choice Panel")]
    [SerializeField] private Button[] choiceButtons;

    [Header("Receipt Panel")]
    [SerializeField] private TMP_Text receiptText;
    [SerializeField] private Button payButton;

    public event Action NextClicked;
    public event Action BackClicked;
    public event Action<int> ChoiceClicked;
    public event Action PayClicked;

    public string InputPumpNumber => pumpNumber.text;
    public string InputLitersNumber => litersCount.text;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextClicked);
        backButton.onClick.AddListener(OnBackClicked);
        payButton.onClick.AddListener(OnPayClicked);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int id = i;
            choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(id));
        }
    }

    private void OnDestroy()
    {
        nextButton.onClick.RemoveListener(OnNextClicked);
        backButton.onClick.RemoveListener(OnBackClicked);
        payButton.onClick.RemoveListener(OnPayClicked);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].onClick.RemoveAllListeners();
        }
    }

    public void ShowRoot()
    {
        root.SetActive(true);
    }

    public void HideRoot()
    {
        root.SetActive(false);
    }

    public void ShowInputPanel()
    {
        inputPanel.SetActive(true);
        choicePanel.SetActive(false);
        receiptPanel.SetActive(false);
    }

    public void ShowChoicePanel()
    {
        inputPanel.SetActive(false);
        choicePanel.SetActive(true);
        receiptPanel.SetActive(false);
    }

    public void ShowReceiptPanel()
    {
        inputPanel.SetActive(false);
        choicePanel.SetActive(false);
        receiptPanel.SetActive(true);
    }

    public void Clear()
    {
        pumpNumber.text = string.Empty;
        litersCount.text = string.Empty;
        receiptText.text = string.Empty;
    }

    public void SetReceiptText(string text)
    {
        receiptText.text = text;
    }

    public void SetPayButtonInteractable(bool value)
    {
        payButton.interactable = value;
    }

    private void OnNextClicked()
    {
        NextClicked?.Invoke();
    }

    private void OnBackClicked()
    {
        BackClicked?.Invoke();
    }

    private void OnChoiceClicked(int id)
    {
        ChoiceClicked?.Invoke(id);
    }

    private void OnPayClicked()
    {
        PayClicked?.Invoke();
    }
}