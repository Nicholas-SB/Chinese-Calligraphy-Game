using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string hanziID;

    private bool playerInRange = false;
    private GameObject interactPrompt;
    private bool isOpen = false;

    void Start()
    {
        interactPrompt = GameObject.FindWithTag("Player")
            .transform.Find("InteractPrompt").gameObject;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen)
            {
                PracticePhaseManager.Instance.CloseCanvas();
                isOpen = false;
            }
            else
            {
                PracticePhaseManager.Instance.OpenCanvas(hanziID);
                isOpen = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        interactPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        isOpen = false;
        interactPrompt.SetActive(false);
        PracticePhaseManager.Instance.CloseCanvas();
    }
}