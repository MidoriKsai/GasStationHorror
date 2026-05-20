using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmartTerminalView : MonoBehaviour
{
    [SerializeField] private GameObject root;

    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject receiptPanel;

    [SerializeField] private TMP_InputField pumpNumber;
    [SerializeField] private TMP_InputField litersCount;

    [SerializeField] private Button[] choiceButtons;

    [SerializeField] private Color normalFuelColor = Color.white;
    [SerializeField] private Color selectedFuelColor = new Color(0.75f, 0.9f, 1f);

    [SerializeField] private Color normalFuelTextColor = Color.black;
    [SerializeField] private Color selectedFuelTextColor = Color.white;

    [SerializeField] private Button[] digitButtons;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button confirmButton;

    [SerializeField] private TMP_Text fuelTypeText;
    [SerializeField] private TMP_Text litersText;
    [SerializeField] private TMP_Text pricePerLiterText;
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private Button payButton;

    public event Action NextClicked;
    public event Action PayClicked;
    public event Action<int> ChoiceClicked;

    public string InputPumpNumber => pumpNumber.text;
    public string InputLitersNumber => litersCount.text;

    private TMP_InputField _activeInput;

    private void Awake()
    {
        _activeInput = pumpNumber;

        SetupInputField(pumpNumber);
        SetupInputField(litersCount);

        pumpNumber.onSelect.AddListener(_ => SetActiveInput(pumpNumber));
        litersCount.onSelect.AddListener(_ => SetActiveInput(litersCount));

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int id = i;
            choiceButtons[i].onClick.AddListener(() => OnChoiceClicked(id));
        }

        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i;
            digitButtons[i].onClick.AddListener(() => AddDigit(digit));
        }

        deleteButton.onClick.AddListener(DeleteLastDigit);
        confirmButton.onClick.AddListener(OnConfirmClicked);

        payButton.onClick.AddListener(OnPayClicked);
    }

    private void OnDestroy()
    {
        if (pumpNumber != null)
            pumpNumber.onSelect.RemoveAllListeners();

        if (litersCount != null)
            litersCount.onSelect.RemoveAllListeners();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] != null)
                choiceButtons[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < digitButtons.Length; i++)
        {
            if (digitButtons[i] != null)
                digitButtons[i].onClick.RemoveAllListeners();
        }

        if (deleteButton != null)
            deleteButton.onClick.RemoveListener(DeleteLastDigit);

        if (confirmButton != null)
            confirmButton.onClick.RemoveListener(OnConfirmClicked);

        if (payButton != null)
            payButton.onClick.RemoveListener(OnPayClicked);
    }

    private void SetupInputField(TMP_InputField inputField)
    {
        if (inputField == null)
            return;

        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.lineType = TMP_InputField.LineType.SingleLine;
        inputField.keyboardType = TouchScreenKeyboardType.NumberPad;
    }

    private void SetActiveInput(TMP_InputField inputField)
    {
        if (inputField == null)
            return;

        _activeInput = inputField;
    }

    private void AddDigit(int digit)
    {
        if (_activeInput == null)
            _activeInput = pumpNumber;

        if (_activeInput == null)
            return;

        _activeInput.text += digit.ToString();
        _activeInput.caretPosition = _activeInput.text.Length;
    }

    private void DeleteLastDigit()
    {
        if (_activeInput == null)
            return;

        if (string.IsNullOrEmpty(_activeInput.text))
            return;

        _activeInput.text = _activeInput.text.Substring(0, _activeInput.text.Length - 1);
        _activeInput.caretPosition = _activeInput.text.Length;
    }

    private void OnChoiceClicked(int id)
    {
        ChoiceClicked?.Invoke(id);
    }

    private void OnConfirmClicked()
    {
        NextClicked?.Invoke();
    }

    private void OnPayClicked()
    {
        PayClicked?.Invoke();
    }

    public void ShowRoot()
    {
        if (root == null)
            return;

        root.SetActive(true);
    }

    public void HideRoot()
    {
        if (root == null)
            return;

        root.SetActive(false);
    }

    public void ShowInputPanel()
    {
        if (inputPanel != null)
            inputPanel.SetActive(true);

        if (receiptPanel != null)
            receiptPanel.SetActive(false);

        SetActiveInput(pumpNumber);
    }

    public void ShowReceiptPanel()
    {
        if (inputPanel != null)
            inputPanel.SetActive(false);

        if (receiptPanel != null)
            receiptPanel.SetActive(true);
    }

    public void Clear()
    {
        if (pumpNumber != null)
        {
            pumpNumber.text = string.Empty;
            pumpNumber.caretPosition = 0;
        }

        if (litersCount != null)
        {
            litersCount.text = string.Empty;
            litersCount.caretPosition = 0;
        }
        
        SetSelectedFuelButton(-1);
        SetActiveInput(pumpNumber);
    }

    public void SetSelectedFuelButton(int selectedId)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] == null)
                continue;

            bool isSelected = i == selectedId;

            Image buttonImage = choiceButtons[i].GetComponent<Image>();

            if (buttonImage != null)
                buttonImage.color = isSelected ? selectedFuelColor : normalFuelColor;

            TMP_Text buttonText = choiceButtons[i].GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
                buttonText.color = isSelected ? selectedFuelTextColor : normalFuelTextColor;
        }
    }

    public void SetReceiptData(
        string fuelType,
        int liters,
        int pricePerLiter,
        int totalPrice)
    {
        if (fuelTypeText != null)
            fuelTypeText.text = fuelType;

        if (litersText != null)
            litersText.text = liters.ToString();

        if (pricePerLiterText != null)
            pricePerLiterText.text = pricePerLiter.ToString();

        if (totalText != null)
            totalText.text = totalPrice.ToString();
    }
}