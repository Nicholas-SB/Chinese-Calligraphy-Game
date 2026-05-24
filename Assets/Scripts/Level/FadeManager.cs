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

    public void FadeToPreviousScene(float duration = 1f)
    {
        int previousIndex = SceneManager.GetActiveScene().buildIndex - 1;
        StartCoroutine(FadeAndLoad(previousIndex, duration));
    }

    public void FadeToNextScene(float duration = 3f)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(FadeAndLoad(nextIndex, duration));
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
}