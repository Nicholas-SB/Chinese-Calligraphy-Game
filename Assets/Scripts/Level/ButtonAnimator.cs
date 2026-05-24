using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private float animSpeed = 10f;

    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    void Awake()
    {
        originalScale = transform.localScale;
        enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTo(originalScale * hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(originalScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ScaleTo(originalScale * pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Bounce slightly past original then settle
        StartCoroutine(Bounce());
    }

    private IEnumerator Bounce()
    {
        // Overshoot
        yield return StartCoroutine(ScaleCoroutine(originalScale * 1.1f));
        // Settle back
        yield return StartCoroutine(ScaleCoroutine(originalScale));
    }

    private void ScaleTo(Vector3 target)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleCoroutine(target));
    }

    private IEnumerator ScaleCoroutine(Vector3 target)
    {
        while (Vector3.Distance(transform.localScale, target) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * animSpeed);
            yield return null;
        }
        transform.localScale = target;
    }

    public void Enable()
    {
        enabled = true;
    }
}