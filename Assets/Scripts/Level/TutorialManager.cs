using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialSlides;
    [SerializeField] private Image dimmer;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private PlayerMovement playerMovement;


    private int currentSlide = 0;
    private bool canAdvance = false;

    void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    void Update()
    {
        if (canAdvance && Input.GetMouseButtonDown(0))
            Advance();
    }

    private IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement.enabled = false;
        tutorialSlides[0].SetActive(true);
        yield return StartCoroutine(FadeIn());
        canAdvance = true;
    }

    private void Advance()
    {
        tutorialSlides[currentSlide].SetActive(false);
        currentSlide++;

        if (currentSlide >= tutorialSlides.Length)
        {
            StartCoroutine(HideTutorial());
            return;
        }

        tutorialSlides[currentSlide].SetActive(true);
    }

    private IEnumerator HideTutorial()
    {
        canAdvance = false;
        yield return StartCoroutine(FadeOut());
        playerMovement.enabled = true;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            dimmer.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.8f, t));
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            dimmer.color = new Color(0f, 0f, 0f, Mathf.Lerp(0.8f, 0f, t));
            yield return null;
        }
    }
}