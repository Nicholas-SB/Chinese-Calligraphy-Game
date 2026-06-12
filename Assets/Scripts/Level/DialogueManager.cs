using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSlide
    {
        public Sprite image;
        [TextArea] public string[] lines;
    }

    [SerializeField] private List<DialogueSlide> slides;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image backgroundImageB;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject clickPrompt;
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private float crossfadeDuration = 0.5f;
    [SerializeField] private float firstSlideAutoAdvanceDelay = 2f;
    [SerializeField] private AudioClip drawingSFX;

    private int currentSlide = 0;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool waitingForInput = false;
    private bool isTransitioning = false;
    private bool usingImageA = true;
    private bool canAdvance = false; // spam guard

    void Start()
    {
        backgroundImageB.color = new Color(1f, 1f, 1f, 0f);
        ShowSlide(0);
    }

    void Update()
    {
        if (!canAdvance) return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            Advance();
    }

    private void ShowSlide(int index)
    {
        if (index >= slides.Count)
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                FadeManager.Instance.FadePlain("0_MainMenu");
            else
                FadeManager.Instance.FadeToNextSceneWithDoor();
            return;
        }

        DialogueSlide slide = slides[index];
        StartCoroutine(CrossfadeToSlide(slide, index));
    }

    private IEnumerator CrossfadeToSlide(DialogueSlide slide, int index)
    {
        canAdvance = false;
        isTransitioning = true;
        dialogueBox.SetActive(false);
        clickPrompt.SetActive(false);

        Image incoming = usingImageA ? backgroundImageB : backgroundImage;
        Image outgoing = usingImageA ? backgroundImage : backgroundImageB;

        incoming.sprite = slide.image;
        incoming.color = new Color(1f, 1f, 1f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / crossfadeDuration;
            incoming.color = new Color(1f, 1f, 1f, Mathf.Clamp01(t));
            outgoing.color = new Color(1f, 1f, 1f, Mathf.Clamp01(1f - t));
            yield return null;
        }

        usingImageA = !usingImageA;
        isTransitioning = false;

        if (index == 0 && (slide.lines == null || slide.lines.Length == 0))
        {
            if (drawingSFX != null)
                AudioSource.PlayClipAtPoint(drawingSFX, Camera.main.transform.position);

            yield return new WaitForSeconds(firstSlideAutoAdvanceDelay);
            currentSlide++;
            ShowSlide(currentSlide);
            yield break;
        }

        if (slide.lines == null || slide.lines.Length == 0)
        {
            clickPrompt.SetActive(true);
            waitingForInput = true;
            canAdvance = true;
        }
        else
        {
            dialogueBox.SetActive(true);
            currentLine = 0;
            StartCoroutine(TypeLine(slide.lines[0]));
        }
    }

    private void Advance()
    {
        if (isTransitioning || !canAdvance) return;

        if (isTyping)
        {
            StopCoroutine("TypeLine");
            isTyping = false;
            dialogueText.text = slides[currentSlide].lines[currentLine];
            clickPrompt.SetActive(true);
            waitingForInput = true;
            return;
        }

        if (!waitingForInput) return;

        waitingForInput = false;
        canAdvance = false;
        clickPrompt.SetActive(false);

        DialogueSlide slide = slides[currentSlide];

        if (slide.lines != null && currentLine + 1 < slide.lines.Length)
        {
            currentLine++;
            StartCoroutine(TypeLine(slide.lines[currentLine]));
        }
        else
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        canAdvance = false;
        dialogueText.text = "";
        clickPrompt.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        canAdvance = true;

        foreach (char c in line)
        {
            if (!isTyping) yield break;
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        clickPrompt.SetActive(true);
        waitingForInput = true;
    }
}