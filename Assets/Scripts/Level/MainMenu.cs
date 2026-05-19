using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

    public void OnPlayButton()
    {
        SceneManager.LoadScene("1_Bedroom");
    }

    public void OnCreditsButton()
    {
        creditsPanel.SetActive(true);
    }

    public void OnCloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void OnSettingsButton()
    {
        // Settings logic will go here later
        Debug.Log("Settings pressed");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}