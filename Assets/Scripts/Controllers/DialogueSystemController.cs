using System.Collections.Generic;
using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace Controllers
{
    public class DialogueSystemController : IController
    {
        private readonly IDialogueView _view;
        private readonly IDialogueStorage _storage;

        private UniTaskCompletionSource<int> _tcs;

        public DialogueSystemController(
            IDialogueView view,
            IDialogueStorage storage)
        {
            _view = view;
            _storage = storage;

            _view.ChoiceSelected += OnChoice;
        }
        
        public UniTask StartDialogueAsync(string id, CancellationToken ct)
        {
            return StartDialogueAsync(id, null, ct);
        }

        public async UniTask StartDialogueAsync(
            string id,
            CustomerData customerData,
            CancellationToken ct)
        {
            var data = await _storage.GetDialogueAsync(id, ct);

            EnableCursor();
            _view.Show();

            foreach (var line in data.Lines)
            {
                string text = ApplyData(line.Text, customerData);

                _view.SetDialogueText(text);

                if (line.Answers.Count == 0)
                {
                    _view.SetChoices(new List<string> { "..." });
                }
                else
                {
                    var choices = new List<string>();

                    foreach (var answer in line.Answers)
                    {
                        choices.Add(answer.Text);
                    }

                    _view.SetChoices(choices);
                }

                _tcs = new UniTaskCompletionSource<int>();

                await _tcs.Task;
            }

            _view.Hide();
            DisableCursor();
        }

        private string ApplyData(string text, CustomerData data)
        {
            if (data == null)
                return text;

            return text
                .Replace("{pump}", data.petrolPumpNumber.ToString())
                .Replace("{liters}", data.literQuantity.ToString())
                .Replace("{fuel}", data.fuelType)
                .Replace("{foodOrder}", BuildFoodOrderText(data));
        }

        private string BuildFoodOrderText(CustomerData data)
        {
            if (data.coffeeCount <= 0 && data.frenchDogCount <= 0)
                return "";

            var parts = new List<string>();

            if (data.coffeeCount > 0)
                parts.Add($"{data.coffeeCount} кофе");

            if (data.frenchDogCount > 0)
            {
                parts.Add($"{data.frenchDogCount} {GetFrenchDogWord(data.frenchDogCount)}");
            }

            return " Ещё " + string.Join(" и ", parts) + ".";
        }
        
        private string GetFrenchDogWord(int count)
        {
            return count == 1 ? "френчдог" : "френчдога";
        }
        
        

        private void OnChoice(int index)
        {
            _tcs?.TrySetResult(index);
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
            _view.ChoiceSelected -= OnChoice;
        }
    }
}