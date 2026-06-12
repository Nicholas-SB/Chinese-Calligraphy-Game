using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        // Set sliders to default values
        masterSlider.value = 0.75f;
        musicSlider.value = 0.75f;
        sfxSlider.value = 0.75f;
    }

    public void SetMasterVolume(float value)
    {
        float db = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("MasterVolume", db);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}