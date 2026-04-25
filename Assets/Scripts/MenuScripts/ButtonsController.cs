using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject mainSection;
    [SerializeField] private GameObject settingsSection;
    [SerializeField] private Slider sensetivitySlider;

    void Start()
    {
        ShowMainSection();
        sensetivitySlider.value = 1f;
    }

    public void LoadMainScene()
    {
        StaticSettings.Sensetivity = sensetivitySlider.value;
        SceneManager.LoadScene("MainScene");
    }

    public void ShowSettingsSection()
    {
        mainSection.SetActive(false);
        settingsSection.SetActive(true);
    }

    public void GoBackToMainSection()
    {
        ShowMainSection();
    }

    public void PerformGameExit()
    {
        Application.Quit();
    }

    public void ShowMainSection()
    {
        mainSection.SetActive(true);
        settingsSection.SetActive(false);
    }
}
