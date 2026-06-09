using UnityEngine;
using System.Collections;

public class SlimeBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer hanziDisplay;

    public Transform player;
    public string[] hanzis;

    private Animator animator;
    public bool isDead = false;
    public bool isFrozen = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        TestingPhaseManager.Instance.RegisterSlime(this);
    }

    void Update()
    {
        if (isDead || isFrozen) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        
        // Flip sprite and mirror HanziDisplay position
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
            hanziDisplay.transform.localPosition = new Vector3(
                -Mathf.Abs(hanziDisplay.transform.localPosition.x),
                hanziDisplay.transform.localPosition.y,
                hanziDisplay.transform.localPosition.z);
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
            hanziDisplay.transform.localPosition = new Vector3(
                Mathf.Abs(hanziDisplay.transform.localPosition.x),
                hanziDisplay.transform.localPosition.y,
                hanziDisplay.transform.localPosition.z);
        }
    }

    public void GetKilled()
    {
        if (isDead) return;
        isDead = true;

        TestingPhaseManager.Instance.UnregisterSlime(this);
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
        
        // Find matching sprite from HanziRecognizer
        var data = HanziRecognizer.Instance.hanziList.Find(h => h.hanziID == hanziSet[0]);
        if (data != null)
            hanziDisplay.sprite = Sprite.Create(data.reference, 
                new Rect(0, 0, data.reference.width, data.reference.height), 
                new Vector2(0.5f, 0.5f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (!other.CompareTag("Player")) return;

        // Notify game manager that player got hit
        TestingPhaseManager.Instance.OnPlayerHit();
    }

    void OnMouseDown()
    {
        if (isDead || isFrozen) return;
        OnTargeted();
    }

    public void OnTargeted()
    {
        TestingPhaseManager.Instance.SetTargetSlime(this);
        TestingPhaseManager.Instance.OnSubmitDrawing();
    }
}