using UnityEngine;

public class SlimeWallTrigger : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        FadeManager.Instance.FadeToNextScene();
    }
}