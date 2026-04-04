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

        public async UniTask StartDialogueAsync(
            string id,
            CancellationToken ct)
        {
            var data = await _storage.GetDialogueAsync(id, ct);
            
            EnableCursor();
            _view.Show();

            foreach (var line in data.Lines)
            {
                _view.SetDialogueText(line.Text);

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