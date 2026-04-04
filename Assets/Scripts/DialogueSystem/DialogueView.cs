using System;
using System.Collections.Generic;
using DialogueSystem.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace DialogueSystem

{
    public class DialogueView : MonoBehaviour, IDialogueView
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text dialogueText;
        
        [SerializeField] private Button[] answerButtons;
        [SerializeField] private TMP_Text[] answerTexts;

        public event Action<int> ChoiceSelected;

        private void Awake()
        {
            SetupButtons();
            Hide();
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetDialogueText(string text)
        {
            dialogueText.text = text;
        }

        public void SetChoices(IReadOnlyList<string> choices)
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                bool shouldShow = i < choices.Count;

                answerButtons[i].gameObject.SetActive(shouldShow);

                if (shouldShow)
                {
                    answerTexts[i].text = choices[i];
                }
            }
        }

        private void SetupButtons()
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() =>
                {
                    ChoiceSelected?.Invoke(index);
                });
            }
        }
    }
}