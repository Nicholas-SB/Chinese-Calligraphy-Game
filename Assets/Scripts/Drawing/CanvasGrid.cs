using UnityEngine;
using UnityEngine.UI;

public class CanvasGrid : MonoBehaviour
{
    [SerializeField] private Color gridColor = new Color(0.8f, 0.2f, 0.2f, 0.5f);
    [SerializeField] private float lineThickness = 2f;

    void Start()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        // Horizontal center line
        CreateLine("HorizontalLine", new Vector2(0, 0), new Vector2(width, 0), false);
        // Vertical center line
        CreateLine("VerticalLine", new Vector2(0, 0), new Vector2(0, height), true);
    }

    private void CreateLine(string name, Vector2 start, Vector2 end, bool vertical)
    {
        GameObject line = new GameObject(name);
        line.transform.SetParent(transform, false);

        Image img = line.AddComponent<Image>();
        img.color = gridColor;

        RectTransform rt = line.GetComponent<RectTransform>();
        RectTransform parentRt = GetComponent<RectTransform>();

        if (vertical)
        {
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.sizeDelta = new Vector2(lineThickness, 0f);
            rt.anchoredPosition = Vector2.zero;
        }
        else
        {
            rt.anchorMin = new Vector2(0f, 0.5f);
            rt.anchorMax = new Vector2(1f, 0.5f);
            rt.sizeDelta = new Vector2(0f, lineThickness);
            rt.anchoredPosition = Vector2.zero;
        }
    }
}