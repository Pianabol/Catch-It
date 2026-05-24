using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header(" Audio Sources ")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource; 

    [Header(" Audio Clips ")]
    [SerializeField] private AudioClip[] squishSounds;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip levelCompleteSound; 
    [SerializeField] private AudioClip gameOverSound;  
    [SerializeField] private AudioClip friendlyPuffSound;

    [Header(" Power Up Clips ")]
    [SerializeField] private AudioClip shieldPowerUpSound;
    [SerializeField] private AudioClip freezePowerUpSound;
    [SerializeField] private AudioClip healPowerUpSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }
    public void PlayPowerUpSound(string powerUpType)
    {
        if (sfxSource == null) return;

        AudioClip clipToPlay = null;

        switch (powerUpType.ToLower())
        {
            case "shield":
                clipToPlay = shieldPowerUpSound;
                break;
            case "freeze":
                clipToPlay = freezePowerUpSound;
                break;
            case "heal":
                clipToPlay = healPowerUpSound;
                break;
        }

        if (clipToPlay != null)
        {
            sfxSource.pitch = Random.Range(0.95f, 1.05f); 
            sfxSource.PlayOneShot(clipToPlay);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && bgmSource != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.loop = true;
            bgmSource.volume = 0.35f; 
            bgmSource.Play();
        }
    }

    
    public void PlayPuffSound()
    {
        if (friendlyPuffSound != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f); // Bu sesi de hafifçe organik yapalım
            sfxSource.PlayOneShot(friendlyPuffSound);
        }
    }

    public void PlaySquishSound()
    {
        if (squishSounds.Length == 0 || sfxSource == null) return;

        AudioClip clip = squishSounds[Random.Range(0, squishSounds.Length)];
        sfxSource.pitch = Random.Range(0.85f, 1.15f); 
        sfxSource.PlayOneShot(clip);
    }

    public void PlayLevelCompleteSound()
    {
        if (levelCompleteSound != null && sfxSource != null)
        {
            if (bgmSource != null) bgmSource.Stop();  
            sfxSource.pitch = 1f;  
            sfxSource.PlayOneShot(levelCompleteSound);
        }
    }

    public void PlayGameOverSound()
    {
        if (gameOverSound != null && sfxSource != null)
        {
            if (bgmSource != null) bgmSource.Stop(); // Arka plan müziğini sustur
            sfxSource.pitch = 1f; 
            sfxSource.PlayOneShot(gameOverSound);
        }
    }
}