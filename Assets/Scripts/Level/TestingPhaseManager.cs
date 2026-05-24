using UnityEngine;
using System.Collections;

public class TestingPhaseManager : MonoBehaviour
{
    public static TestingPhaseManager Instance;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float hitWaitDuration = 2f;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    public void OnPlayerHit()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop spawner first
        SlimeSpawner spawner = FindFirstObjectByType<SlimeSpawner>();
        if (spawner != null)
            spawner.enabled = false;

        // Freeze all existing slimes
        SlimeBehavior[] allSlimes = FindObjectsByType<SlimeBehavior>(FindObjectsSortMode.None);
        foreach (SlimeBehavior slime in allSlimes)
            slime.Freeze();

        StartCoroutine(HitSequence());
    }

    private IEnumerator HitSequence()
    {
        // Trigger hit animation
        playerAnimator.SetTrigger("Hit");

        // Wait 2 seconds
        yield return new WaitForSeconds(hitWaitDuration);

        // Fade back to practice scene
        FadeManager.Instance.FadeToPreviousScene();
    }
}