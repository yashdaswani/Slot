using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource uiSoundSource;
    [SerializeField] private AudioSource gameplaySoundSource;

    [SerializeField] private AudioClip uiButtonClip;
    [SerializeField] private AudioClip spinReelClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

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

    public void PlayUIButtonSound()
    {
        if (uiSoundSource != null && uiButtonClip != null)
        {
            uiSoundSource.PlayOneShot(uiButtonClip);
        }
    }

    public void PlaySpinReelSound()
    {
        if (gameplaySoundSource != null && spinReelClip != null)
        {
            gameplaySoundSource.clip = spinReelClip;
            gameplaySoundSource.loop = true;
            gameplaySoundSource.Play();
        }
    }

    public void StopSpinReelSound()
    {
        if (gameplaySoundSource != null)
        {
            gameplaySoundSource.Stop();
            gameplaySoundSource.loop = false;
        }
    }

    public void PlayWinSound()
    {
        if (gameplaySoundSource != null && winClip != null)
        {
            gameplaySoundSource.PlayOneShot(winClip);
        }
    }

    public void PlayLoseSound()
    {
        if (gameplaySoundSource != null && loseClip != null)
        {
            gameplaySoundSource.PlayOneShot(loseClip);
        }
    }
}