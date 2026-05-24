using UnityEngine;
using System.Collections;

public class SlimeBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Transform player;
    public string[] hanzis;

    private Animator animator;
    public bool isDead = false;
    public bool isFrozen = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isDead || isFrozen) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        spriteRenderer.flipX = direction.x > 0;
    }

    public void GetKilled()
    {
        if (isDead) return;
        isDead = true;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        animator.SetTrigger("Explode");
        StartCoroutine(DestroyAfterAnimation());
    }

    public void Freeze()
    {
        isFrozen = true;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public void SetHanzis(string[] hanziSet)
    {
        hanzis = hanziSet;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (!other.CompareTag("Player")) return;

        // Notify game manager that player got hit
        TestingPhaseManager.Instance.OnPlayerHit();
    }
}