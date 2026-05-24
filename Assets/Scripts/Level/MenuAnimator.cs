using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private CanvasGroup playButton;
    [SerializeField] private CanvasGroup settingsButton;
    [SerializeField] private CanvasGroup quitButton;
    [SerializeField] private CanvasGroup creditsButton;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float delayBetweenButtons = 0.2f;

    void Start()
    {
        StartCoroutine(FadeButtonsIn());
    }

    private IEnumerator FadeButtonsIn()
    {
        // Wait for title to finish dropping first
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeIn(playButton));
        yield return new WaitForSeconds(delayBetweenButtons);

        yield return StartCoroutine(FadeIn(settingsButton));
        yield return new WaitForSeconds(delayBetweenButtons);

        yield return StartCoroutine(FadeIn(quitButton));
        yield return new WaitForSeconds(delayBetweenButtons);

        yield return StartCoroutine(FadeIn(creditsButton));
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        group.interactable = false;
        group.blocksRaycasts = false;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            group.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;

        // Enable button animation after fade
        ButtonAnimator animator = group.GetComponent<ButtonAnimator>();
        if (animator != null)
            animator.Enable();
    }
}