using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PracticePhaseManager : MonoBehaviour
{
    public static PracticePhaseManager Instance;

    [System.Serializable]
    public class HanziDisplay
    {
        public string hanziID;
        public Texture2D texture;
        public AudioClip voiceLine;
    }

    [SerializeField] private List<HanziDisplay> hanziList;
    [SerializeField] private GameObject canvasPanel;
    [SerializeField] private RawImage hanziGuide;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AudioManager.Instance.PlayPractice();
    }

    public void OpenCanvas(string hanziID)
    {
        var data = hanziList.Find(h => h.hanziID == hanziID);
        if (data != null)
        {
            hanziGuide.texture = data.texture;
            hanziGuide.color = new Color(1f, 1f, 1f, 0.8f);

            // Play voice line
            if (data.voiceLine != null)
                AudioManager.Instance.PlayVoiceLine(data.voiceLine);
        }

        canvasPanel.SetActive(true);
    }

    public void CloseCanvas()
    {
        canvasPanel.SetActive(false);
        hanziGuide.texture = null;
    }
}