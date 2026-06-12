using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip practiceMusic;
    [SerializeField] private AudioClip testingMusic;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip doorOpenSFX;
    [SerializeField] private AudioClip doorCloseSFX;
    [SerializeField] private AudioClip failSFX;
    [SerializeField] private AudioClip successSFX;

    [Header("VA")]
    [SerializeField] private AudioSource voiceSource;

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

    public void PlayMainMenu() => PlayMusic(mainMenuMusic);
    public void PlayPractice() => PlayMusic(practiceMusic);
    public void PlayTesting() => PlayMusic(testingMusic);

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayVoiceLine(AudioClip clip)
    {
        voiceSource.Stop();
        voiceSource.PlayOneShot(clip);
    }

    public void PlayPlayerHit() => sfxSource.PlayOneShot(playerHitSFX);
    public void PlayDoorOpen() => sfxSource.PlayOneShot(doorOpenSFX);
    public void PlayDoorClose() => sfxSource.PlayOneShot(doorCloseSFX);
    public void PlayFail() => sfxSource.PlayOneShot(failSFX);
    public void PlaySuccess() => sfxSource.PlayOneShot(successSFX);
}