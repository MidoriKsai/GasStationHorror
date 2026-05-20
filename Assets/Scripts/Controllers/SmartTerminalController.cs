using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Interactables.SmartTerminal;
using Services.Interfaces;
using UnityEngine;

public class SmartTerminalController : IController
{
    private readonly SmartTerminalView _view;
    private readonly SmartTerminalInteractable _interactable;
    private readonly IPlayerService _playerService;
    private readonly Transform _focusPoint;

    private CustomerData _currentCustomerData;

    private int _enteredPumpNumber;
    private int _enteredLiterQuantity;
    private int _selectedFuelId = -1;

    private bool _inputDataValid;

    private UniTaskCompletionSource<bool> _resultTcs;
    private bool _isSessionActive;

    public SmartTerminalController(
        SmartTerminalView view,
        SmartTerminalInteractable interactable,
        IPlayerService playerService,
        Transform focusPoint)
    {
        _view = view;
        _interactable = interactable;
        _playerService = playerService;
        _focusPoint = focusPoint;

        _view.NextClicked += OnNextClicked;
        _view.ChoiceClicked += OnChoiceClicked;
        _view.PayClicked += OnPayClicked;

        _view.HideRoot();
        _isSessionActive = false;
    }

    public void InitializeInteract()
    {
        _interactable.Initialize(OpenByInteract);
        _interactable.SetAvailable(false);
    }

    public void EnableInteraction(CustomerData customerData)
    {
        _currentCustomerData = customerData;
        _isSessionActive = false;

        if (_resultTcs == null || _resultTcs.Task.Status.IsCompleted())
        {
            _resultTcs = new UniTaskCompletionSource<bool>();
        }

        _interactable.SetAvailable(true);
    }

    public void DisableInteraction()
    {
        _isSessionActive = false;
        _interactable.SetAvailable(false);
        _view.HideRoot();

        DisableCursor();
        _playerService.UnfocusPlayerFromDialogue();
    }

    public UniTask<bool> WaitForResultAsync()
    {
        if (_resultTcs == null || _resultTcs.Task.Status.IsCompleted())
        {
            _resultTcs = new UniTaskCompletionSource<bool>();
        }

        return _resultTcs.Task;
    }

    private void OpenByInteract()
    {
        if (_currentCustomerData == null)
            return;

        if (_isSessionActive)
            return;

        _isSessionActive = true;
        _interactable.SetAvailable(false);

        _enteredPumpNumber = -1;
        _enteredLiterQuantity = -1;
        _selectedFuelId = -1;
        _inputDataValid = false;

        EnableCursor();
        _playerService.FocusPlayerToDialogue(_focusPoint);

        _view.Clear();
        _view.ShowRoot();
        _view.ShowInputPanel();
    }

    private void OnChoiceClicked(int buttonId)
    {
        if (!_isSessionActive || _currentCustomerData == null)
            return;

        _selectedFuelId = buttonId;
        _view.SetSelectedFuelButton(_selectedFuelId);
    }

    private void OnNextClicked()
    {
        if (!_isSessionActive || _currentCustomerData == null)
            return;

        bool pumpParsed = int.TryParse(_view.InputPumpNumber, out _enteredPumpNumber);
        bool litersParsed = int.TryParse(_view.InputLitersNumber, out _enteredLiterQuantity);

        int correctPumpNumber = _currentCustomerData.petrolPumpNumber + 1;
        int correctLiterQuantity = _currentCustomerData.literQuantity;
        int correctFuelId = _currentCustomerData.patrolId;

        if (!pumpParsed || !litersParsed)
        {
            DebugCorrectData(correctPumpNumber, correctLiterQuantity, correctFuelId);
            return;
        }

        bool pumpCorrect = _enteredPumpNumber == correctPumpNumber;
        bool litersCorrect = _enteredLiterQuantity == correctLiterQuantity;
        bool fuelCorrect = _selectedFuelId == correctFuelId;

        _inputDataValid = pumpCorrect && litersCorrect && fuelCorrect;

        if (!_inputDataValid)
        {
            DebugCorrectData(correctPumpNumber, correctLiterQuantity, correctFuelId);

            Debug.Log($"Введённая колонка: {_enteredPumpNumber}");
            Debug.Log($"Введённые литры: {_enteredLiterQuantity}");
            Debug.Log($"Выбранный ID топлива: {_selectedFuelId}");

            return;
        }

        ShowReceipt();
    }

    private void ShowReceipt()
    {
        string fuelType = _currentCustomerData.fuelType;
        int liters = _currentCustomerData.literQuantity;

        int pricePerLiter = GetPricePerLiter();
        int totalPrice = liters * pricePerLiter;

        _view.SetReceiptData(
            fuelType,
            liters,
            pricePerLiter,
            totalPrice);

        _view.ShowReceiptPanel();
    }

    private int GetPricePerLiter()
    {
        switch (_currentCustomerData.patrolId)
        {
            case 0:
                return 50;

            case 1:
                return 55;

            case 2:
                return 65;

            case 3:
                return 60;

            default:
                return 0;
        }
    }

    private void OnPayClicked()
    {
        if (!_isSessionActive)
            return;

        if (!_inputDataValid)
            return;

        FinishSession();
    }

    private void FinishSession()
    {
        CloseSmartTerminalPanel();

        _interactable.SetAvailable(false);

        _resultTcs?.TrySetResult(true);
    }

    private void CloseSmartTerminalPanel()
    {
        _view.HideRoot();

        DisableCursor();
        _playerService.UnfocusPlayerFromDialogue();

        _isSessionActive = false;
    }

    private void DebugCorrectData(
        int correctPumpNumber,
        int correctLiterQuantity,
        int correctFuelId)
    {
        Debug.Log($"Правильная колонка: {correctPumpNumber}");
        Debug.Log($"Правильные литры: {correctLiterQuantity}");
        Debug.Log($"Правильный ID топлива: {correctFuelId}");
    }

    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Dispose()
    {
        _view.NextClicked -= OnNextClicked;
        _view.ChoiceClicked -= OnChoiceClicked;
        _view.PayClicked -= OnPayClicked;
    }
}