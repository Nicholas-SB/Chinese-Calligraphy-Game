using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TestingPhaseManager : MonoBehaviour
{
    public static TestingPhaseManager Instance;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float hitWaitDuration = 2f;
    [SerializeField] private DrawingCanvas drawingCanvas;
    [SerializeField] private TextMeshProUGUI feedbackText;

    private SlimeBehavior targetedSlime;
    private bool isGameOver = false;
    private List<SlimeBehavior> activeSlimes = new List<SlimeBehavior>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterSlime(SlimeBehavior slime)
    {
        activeSlimes.Add(slime);
    }

    public void UnregisterSlime(SlimeBehavior slime)
    {
        activeSlimes.Remove(slime);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        SlimeSpawner spawner = FindFirstObjectByType<SlimeSpawner>();
        bool doneSpawning = spawner == null || !spawner.enabled || 
                            spawner.spawnedCount >= spawner.maxSlimes;

        if (doneSpawning && activeSlimes.Count == 0)
            StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        yield return new WaitForSeconds(3f);
        FadeManager.Instance.FadeToNextScene();
    }

    public void SetTargetSlime(SlimeBehavior slime)
    {
        if (isGameOver) return;
        targetedSlime = slime;
        Debug.Log($"Targeted slime: {slime.hanzis[0]}");
    }

    public void OnSubmitDrawing()
    {
        if (isGameOver) return;

        if (targetedSlime == null)
        {
            StartCoroutine(ShowFeedback("Tap a slime first!"));
            return;
        }

        Texture2D drawing = drawingCanvas.GetDrawing();
        string targetHanzi = targetedSlime.hanzis[0];
        float score = HanziRecognizer.Instance.RecognizeSingle(drawing, targetHanzi);

        Debug.Log($"Score against {targetHanzi}: {score * 100f:F1}%");

        if (score >= HanziRecognizer.Instance.passThreshold)
        {
            targetedSlime.GetKilled();
            targetedSlime = null;
            drawingCanvas.ClearCanvas();
            playerAnimator.SetTrigger("Cast");
        }
        else
        {
            StartCoroutine(ShowFeedback($"{score * 100f:F0}% - Try again!"));
        }
    }

    private IEnumerator ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        feedbackText.gameObject.SetActive(false);
    }

    public void OnPlayerHit()
    {
        if (isGameOver) return;
        isGameOver = true;

        SlimeSpawner spawner = FindFirstObjectByType<SlimeSpawner>();
        if (spawner != null)
            spawner.enabled = false;

        SlimeBehavior[] allSlimes = FindObjectsByType<SlimeBehavior>(FindObjectsSortMode.None);
        foreach (SlimeBehavior slime in allSlimes)
            slime.Freeze();

        StartCoroutine(HitSequence());
    }

    private IEnumerator HitSequence()
    {
        playerAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(hitWaitDuration);

        if (FadeManager.Instance != null)
            FadeManager.Instance.FadeToPreviousScene();
        else
            Debug.LogWarning("FadeManager not found! Start from 0_MainMenu.");
    }

    
}

