using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fadeCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // For transitioning into practice scenes (with door SFX)
    public void FadeToScene(string sceneName, float duration = 1f)
    {
        StartCoroutine(DoorAndFade(sceneName, duration));
    }

    // For transitioning into testing scenes (silent, meditative)
    public void FadeToNextScene(float duration = 3f)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(FadeAndLoad(nextIndex, duration));
    }

    // Failing testing phase -> back to practice (with door SFX)
    public void FadeToPreviousScene(float duration = 1f)
    {
        int previousIndex = SceneManager.GetActiveScene().buildIndex - 1;
        StartCoroutine(DoorAndFade(previousIndex, duration));
    }

    private IEnumerator DoorAndFade(string sceneName, float duration)
    {
        // Door open SFX
        AudioManager.Instance.PlayDoorOpen();
        yield return new WaitForSeconds(0.5f);

        // Door close SFX then fade
        AudioManager.Instance.PlayDoorClose();
        yield return StartCoroutine(Fade(0f, 1f, duration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }

    private IEnumerator DoorAndFade(int buildIndex, float duration)
    {
        AudioManager.Instance.PlayDoorOpen();
        yield return new WaitForSeconds(0.5f);

        AudioManager.Instance.PlayDoorClose();
        yield return StartCoroutine(Fade(0f, 1f, duration));
        SceneManager.LoadScene(buildIndex);
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }

    private IEnumerator FadeAndLoad(int buildIndex, float duration)
    {
        yield return StartCoroutine(Fade(0f, 1f, duration));
        SceneManager.LoadScene(buildIndex);
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            fadeCanvas.alpha = Mathf.Clamp01(Mathf.Lerp(from, to, t));
            yield return null;
        }
    }

    public void FadeToNextSceneWithDoor(float duration = 1f)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(DoorAndFade(nextIndex, duration));
    }

    public void FadePlain(string sceneName, float duration = 1f)
    {
        StartCoroutine(FadeAndLoad(sceneName, duration));
    }

    private IEnumerator FadeAndLoad(string sceneName, float duration)
    {
        yield return StartCoroutine(Fade(0f, 1f, duration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }
}