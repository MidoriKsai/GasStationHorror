using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueView : MonoBehaviour, IDialogueView
    {
        [SerializeField] private GameObject root;

        [SerializeField] private TMP_Text dialogueText;

        [SerializeField] private Button[] answerButtons;
        [SerializeField] private TMP_Text[] answerTexts;

        [SerializeField] private GameObject[] activeAnswerImages;

        [SerializeField] private float typingSpeed = 0.03f;

        private int currentChoiceIndex;
        private int visibleChoicesCount;

        private Coroutine typingCoroutine;

        public event Action<int> ChoiceSelected;

        private void Awake()
        {
            SetupButtons();
            Hide();
        }

        private void Update()
        {
            if (!root.activeSelf || visibleChoicesCount == 0)
                return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                currentChoiceIndex--;

                if (currentChoiceIndex < 0)
                {
                    currentChoiceIndex = visibleChoicesCount - 1;
                }

                UpdateChoiceVisual();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                currentChoiceIndex++;

                if (currentChoiceIndex >= visibleChoicesCount)
                {
                    currentChoiceIndex = 0;
                }

                UpdateChoiceVisual();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ChoiceSelected?.Invoke(currentChoiceIndex);
            }
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        public void SetDialogueText(string text)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeText(text));
        }

        private IEnumerator TypeText(string text)
        {
            dialogueText.text = text;

            dialogueText.maxVisibleCharacters = 0;

            for (int i = 0; i <= text.Length; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                yield return new WaitForSeconds(typingSpeed);
            }

            typingCoroutine = null;
        }

        public void SetChoices(IReadOnlyList<string> choices)
        {
            visibleChoicesCount = choices.Count;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                bool shouldShow = i < choices.Count;

                answerButtons[i].gameObject.SetActive(shouldShow);

                if (shouldShow)
                {
                    answerTexts[i].text = choices[i];
                }

                if (i < activeAnswerImages.Length)
                {
                    activeAnswerImages[i].SetActive(false);
                }
            }

            currentChoiceIndex = 0;

            UpdateChoiceVisual();
        }

        private void UpdateChoiceVisual()
        {
            for (int i = 0; i < activeAnswerImages.Length; i++)
            {
                if (!answerButtons[i].gameObject.activeSelf)
                    continue;

                activeAnswerImages[i].SetActive(i == currentChoiceIndex);
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
                    currentChoiceIndex = index;

                    UpdateChoiceVisual();

                    ChoiceSelected?.Invoke(index);
                });

                EventTrigger trigger =
                    answerButtons[i].GetComponent<EventTrigger>();

                if (trigger == null)
                {
                    trigger =
                        answerButtons[i].gameObject.AddComponent<EventTrigger>();
                }

                EventTrigger.Entry pointerEnter =
                    new EventTrigger.Entry();

                pointerEnter.eventID =
                    EventTriggerType.PointerEnter;

                pointerEnter.callback.AddListener((_) =>
                {
                    currentChoiceIndex = index;

                    UpdateChoiceVisual();
                });

                trigger.triggers.Add(pointerEnter);
            }
        }
    }
}