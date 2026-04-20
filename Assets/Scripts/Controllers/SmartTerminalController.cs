using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Interactables.SmartTerminal;
using UnityEngine;

public class SmartTerminalController: IController
{
    private readonly SmartTerminalView _view;
    private readonly SmartTerminalInteractable _interactable;

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
        SmartTerminalInteractable interactable)
    {
        _view = view;
        _interactable = interactable;

        _view.NextClicked += OnNextClicked;
        _view.BackClicked += OnBackClicked;
        _view.ChoiceClicked += OnChoiceClicked;
        _view.PayClicked += OnPayClicked;
    }

    public void InitializeInteract()
    {
        _interactable.Initialize(OpenByInteract);
        _interactable.SetAvailable(false);
    }

    public void EnableInteraction(CustomerData customerData)
    {
        _currentCustomerData = customerData;
        _interactable.SetAvailable(true);
    }

    public void DisableInteraction()
    {
        _interactable.SetAvailable(false);
    }

    public UniTask<bool> WaitForResultAsync()
    {
        _resultTcs = new UniTaskCompletionSource<bool>();
        return _resultTcs.Task;
    }

    private void OpenByInteract()
    {
        if (_currentCustomerData == null)
        {
            return;
        }

        if (_isSessionActive)
        {
            return;
        }

        _isSessionActive = true;

        _enteredPumpNumber = -1;
        _enteredLiterQuantity = -1;
        _selectedButtonId = -1;
        _inputsValid = false;
        _choiceValid = false;
        
        EnableCursor();

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

        if (!pumpParsed || !litersParsed)
        {
            Debug.Log($"Колонка: {_currentCustomerData.petrolPumpNumber}");
            Debug.Log($"Литры: {_currentCustomerData.literQuantity}");
            return;
        }

        bool firstCorrect = _enteredPumpNumber == _currentCustomerData.petrolPumpNumber;
        bool secondCorrect = _enteredLiterQuantity == _currentCustomerData.literQuantity;

        _inputsValid = firstCorrect && secondCorrect;

        if (!_inputsValid)
        {

            Debug.Log($"Колонка: {_currentCustomerData.petrolPumpNumber}");
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

        string receiptText =
            $"Клиент: {_currentCustomerData.customerId}\n" +
            $"Колонка: {_currentCustomerData.petrolPumpNumber}\n" +
            $"Литры: {_currentCustomerData.literQuantity}\n" +
            $"Кнопка: {_selectedButtonId}\n";

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
        {
            return;
        }

        FinishSession(true);
    }

    private void OnBackClicked()
    {
        if (!_isSessionActive)
            return;
        
        FinishSession(false);
    }

    private void FinishSession(bool result)
    {
        _view.HideRoot();
        DisableCursor();
        _interactable.SetAvailable(false);
        _isSessionActive = false;
        _resultTcs?.TrySetResult(result);
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