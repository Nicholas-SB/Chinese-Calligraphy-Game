using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private float flipSpeed = 10f;

    private bool facingRight = false;
    private Coroutine flipCoroutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        // animation speed
        if (Mathf.Abs(rb.linearVelocity.x) > 0.01f)
            animator.speed = 1f;
        else
            animator.speed = 0.5f;

        // sprite flipping
        if (rb.linearVelocity.x > 0.01f && !facingRight)
        {
            facingRight = true;
            StartFlip(-1f);
        }
        else if (rb.linearVelocity.x < -0.01f && facingRight)
        {
            facingRight = false;
            StartFlip(1f);
        }
    }

    private void StartFlip(float targetX)
    {
        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(SmoothFlip(targetX));
    }

    private System.Collections.IEnumerator SmoothFlip(float targetX)
    {
        float startX = transform.localScale.x;
        Vector3 scale = transform.localScale;

        // squish to 0 from current position
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flipSpeed;
            scale.x = Mathf.Lerp(startX, 0f, t);
            transform.localScale = scale;
            yield return null;
        }

        // unsquish to target
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * flipSpeed;
            scale.x = Mathf.Lerp(0f, targetX, t);
            transform.localScale = scale;
            yield return null;
        }

        flipCoroutine = null;
    }

    public void SetDrawing(bool isDrawing)
    {
        animator.SetBool("isDrawing", isDrawing);
    }
}