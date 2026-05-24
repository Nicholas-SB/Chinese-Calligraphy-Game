using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject dimmer;

    public void OnPlayButton()
    {
        SceneManager.LoadScene("1_Bedroom");
    }

    public void OnCreditsButton()
    {
        dimmer.SetActive(true);
        creditsPanel.SetActive(true);
    }

    public void OnCloseCredits()
    {
        dimmer.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void OnSettingsButton()
    {
        dimmer.SetActive(true);
        settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        dimmer.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}