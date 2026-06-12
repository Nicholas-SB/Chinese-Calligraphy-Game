using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestingPhaseManager : MonoBehaviour
{
    public static TestingPhaseManager Instance;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float hitWaitDuration = 2f;
    [SerializeField] private DrawingCanvas drawingCanvas;

    private SlimeBehavior targetedSlime;
    private bool isGameOver = false;
    private List<SlimeBehavior> activeSlimes = new List<SlimeBehavior>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AudioManager.Instance.PlayTesting();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(WinSequence());
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
        FadeManager.Instance.FadeToNextSceneWithDoor();
    }

    public void SetTargetSlime(SlimeBehavior slime)
    {
        if (isGameOver) return;
        targetedSlime = slime;
    }

    public void OnSubmitDrawing()
    {
        if (isGameOver || targetedSlime == null) return;

        Texture2D drawing = drawingCanvas.GetDrawing();
        string targetHanzi = targetedSlime.hanzis[0];
        float score = HanziRecognizer.Instance.RecognizeSingle(drawing, targetHanzi);

        Debug.Log($"Score against {targetHanzi}: {score * 100f:F1}%");

        if (score >= HanziRecognizer.Instance.passThreshold)
            {
                AudioManager.Instance.PlaySuccess();
                targetedSlime.GetKilled();
                targetedSlime = null;
                drawingCanvas.ClearCanvas();
                playerAnimator.SetTrigger("Cast");
            }
            else
            {
                StartCoroutine(FailSequence());
            }
    }

    private IEnumerator FailSequence()
    {
        AudioManager.Instance.PlayFail();
        yield return new WaitForSeconds(0.5f);
        drawingCanvas.ClearCanvas();
        targetedSlime = null;
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
    }
}