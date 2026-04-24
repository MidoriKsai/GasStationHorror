using TMPro;
using TipsSystem.Interfaces;
using UnityEngine;

namespace TipsSystem
{
    public class TipView : MonoBehaviour, ITipView
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text tipText;

        private void Awake()
        {
            Hide();
        }

        public void Show(string text)
        {
            tipText.text = text;
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}