using System;
using System.Collections.Generic;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueView
    {
        event Action<int> ChoiceSelected;

        void Show();
        void Hide();

        void SetDialogueText(string text);
        void SetChoices(IReadOnlyList<string> choices);
    }
}