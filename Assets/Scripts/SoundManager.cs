using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound effects")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip collectibleSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip bossHitSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.3f;

    public static SoundManager Instance;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (Instance != this) return;

        PlayMusic();
    }

    private void PlayMusic()
    {
        if (backgroundMusic == null) return;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.PlayOneShot(clip);
    }

    public void PlayJumpSound() { PlaySound(jumpSound); }

    public void PlayCollectibleSound() { PlaySound(collectibleSound); }

    public void PlayShootSound() { PlaySound(shootSound); }

    public void PlayHurtSound() { PlaySound(hurtSound); }

    public void PlayBossHitSound() { PlaySound(bossHitSound); }

    public void PlayWinSound() { PlaySound(winSound); }
}
