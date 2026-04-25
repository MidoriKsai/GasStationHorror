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
    private int _selectedButtonId = -1;

    private bool _inputsValid;
    private bool _choiceValid;

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
        _view.BackClicked += OnBackClicked;
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
        _selectedButtonId = -1;
        _inputsValid = false;
        _choiceValid = false;

        EnableCursor();
        _playerService.FocusPlayerToDialogue(_focusPoint);

        _view.Clear();
        _view.ShowRoot();
        _view.ShowInputPanel();
        _view.SetPayButtonInteractable(false);
    }

    private void OnNextClicked()
    {
        if (!_isSessionActive || _currentCustomerData == null)
            return;

        bool pumpParsed = int.TryParse(_view.InputPumpNumber, out _enteredPumpNumber);
        bool litersParsed = int.TryParse(_view.InputLitersNumber, out _enteredLiterQuantity);

        int correctPumpNumber = _currentCustomerData.petrolPumpNumber + 1;

        if (!pumpParsed || !litersParsed)
        {
            Debug.Log($"Колонка: {correctPumpNumber}");
            Debug.Log($"Литры: {_currentCustomerData.literQuantity}");
            return;
        }

        bool firstCorrect = _enteredPumpNumber == correctPumpNumber;
        bool secondCorrect = _enteredLiterQuantity == _currentCustomerData.literQuantity;

        _inputsValid = firstCorrect && secondCorrect;

        if (!_inputsValid)
        {
            Debug.Log($"Колонка: {correctPumpNumber}");
            Debug.Log($"Литры: {_currentCustomerData.literQuantity}");
            return;
        }

        _view.ShowChoicePanel();
    }

    private void OnChoiceClicked(int buttonId)
    {
        if (!_isSessionActive || _currentCustomerData == null)
            return;

        _selectedButtonId = buttonId;
        _choiceValid = _selectedButtonId == _currentCustomerData.patrolId;

        if (!_choiceValid)
        {
            Debug.Log($"Id кнопки: {_currentCustomerData.patrolId}");
            return;
        }

        int receiptPumpNumber = _currentCustomerData.petrolPumpNumber + 1;

        string receiptText =
            $"Колонка: {receiptPumpNumber}\n" +
            $"Литры: {_currentCustomerData.literQuantity}\n" +
            $"Топливо: {_currentCustomerData.fuelType}\n";

        _view.SetReceiptText(receiptText);
        _view.SetPayButtonInteractable(true);
        _view.ShowReceiptPanel();
    }

    private void OnPayClicked()
    {
        if (!_isSessionActive)
            return;

        bool result = _inputsValid && _choiceValid;

        if (!result)
            return;

        FinishSession();
    }

    private void OnBackClicked()
    {
        if (!_isSessionActive)
            return;

        CloseSmartTerminalPanel();
        _interactable.SetAvailable(true);
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
        _view.BackClicked -= OnBackClicked;
        _view.ChoiceClicked -= OnChoiceClicked;
        _view.PayClicked -= OnPayClicked;
    }
}