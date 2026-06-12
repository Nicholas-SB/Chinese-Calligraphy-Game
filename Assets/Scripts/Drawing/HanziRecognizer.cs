using UnityEngine;
using System.Collections.Generic;

public class HanziRecognizer : MonoBehaviour
{
    public static HanziRecognizer Instance;

    [System.Serializable]
    public class HanziData
    {
        public string hanziID;        // e.g. "book", "bed"
        public Texture2D reference;   // 512x512 reference texture
    }

    [SerializeField] public List<HanziData> hanziList = new List<HanziData>();
    public float passThreshold = 0.3f;
    [SerializeField] private int compareResolution = 128;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float CompareTextures(Texture2D drawing, Texture2D reference)
    {
        Color[] drawPixels = drawing.GetPixels();
        Color[] refPixels = reference.GetPixels();

        int refInk = 0;
        int overlap = 0;

        int refStepX = reference.width / compareResolution;
        int refStepY = reference.height / compareResolution;
        int drawStepX = drawing.width / compareResolution;
        int drawStepY = drawing.height / compareResolution;

        for (int x = 0; x < compareResolution; x++)
        {
            for (int y = 0; y < compareResolution; y++)
            {
                int refIdx = (y * refStepY) * reference.width + (x * refStepX);
                int drawIdx = (y * drawStepY) * drawing.width + (x * drawStepX);

                bool refIsInk = refPixels[refIdx].a > 0.1f;
                bool drawIsInk = drawPixels[drawIdx].a > 0.1f;

                if (refIsInk)
                {
                    refInk++;
                    if (drawIsInk)
                        overlap++;
                }
            }
        }

        if (refInk == 0) return 0f;
        return (float)overlap / refInk;
    }

    public float RecognizeSingle(Texture2D drawing, string hanziID)
    {
        HanziData data = hanziList.Find(h => h.hanziID == hanziID);
        if (data == null)
        {
            Debug.LogWarning($"Hanzi {hanziID} not found in list!");
            return 0f;
        }

        return CompareTextures(drawing, data.reference);
    }
}