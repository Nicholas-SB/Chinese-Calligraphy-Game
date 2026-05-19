using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DrawingCanvas : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private int brushSize = 15;
    [SerializeField] private Color brushColor = Color.black;

    private Texture2D drawTexture;
    private Vector2 lastPos;
    private bool isDrawing = false;
    private RectTransform rectTransform;

    // Stroke data for ML Kit
    private List<List<Vector2>> strokes = new List<List<Vector2>>();
    private List<Vector2> currentStroke = new List<Vector2>();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        drawTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        rawImage.texture = drawTexture;
        ClearCanvas();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;

        // Start a new stroke
        currentStroke = new List<Vector2>();

        lastPos = GetTexturePosition(eventData.position, eventData.pressEventCamera);
        currentStroke.Add(lastPos);

        DrawCircle(lastPos, brushSize);
        drawTexture.Apply();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrawing) return;

        Vector2 currentPos = GetTexturePosition(eventData.position, eventData.pressEventCamera);

        // Record point in current stroke
        currentStroke.Add(currentPos);

        DrawLine(lastPos, currentPos);
        lastPos = currentPos;
        drawTexture.Apply();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;

        // Save completed stroke
        if (currentStroke.Count > 0)
            strokes.Add(new List<Vector2>(currentStroke));

        currentStroke.Clear();
    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        float dist = Vector2.Distance(from, to);
        for (float i = 0; i <= dist; i += brushSize * 0.3f)
        {
            float t = i / dist;
            Vector2 point = Vector2.Lerp(from, to, t);
            DrawCircle(point, brushSize);
        }
    }

    private void DrawCircle(Vector2 center, int radius)
    {
        int cx = (int)center.x;
        int cy = (int)center.y;

        for (int x = cx - radius; x <= cx + radius; x++)
        {
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                if (x < 0 || x >= drawTexture.width || y < 0 || y >= drawTexture.height) continue;

                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                if (dist <= radius)
                    drawTexture.SetPixel(x, y, brushColor);
            }
        }
    }

    private Vector2 GetTexturePosition(Vector2 screenPos, Camera cam)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPos, cam, out Vector2 localPos
        );

        float normalizedX = (localPos.x + rectTransform.rect.width * 0.5f) / rectTransform.rect.width;
        float normalizedY = (localPos.y + rectTransform.rect.height * 0.5f) / rectTransform.rect.height;

        return new Vector2(normalizedX * drawTexture.width, normalizedY * drawTexture.height);
    }

    public List<List<Vector2>> GetStrokes()
    {
        return strokes;
    }

    public void ClearCanvas()
    {
        Color[] pixels = new Color[drawTexture.width * drawTexture.height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        drawTexture.SetPixels(pixels);
        drawTexture.Apply();

        // Clear stroke data too
        strokes.Clear();
        currentStroke.Clear();
    }
}