using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;

    [Header("Sound effects (8 событий)")]
    [SerializeField] private AudioClip[] jumpSounds;          
    [SerializeField] private AudioClip[] hurtSounds;          
    [SerializeField] private AudioClip[] collectibleSounds;   
    [SerializeField] private AudioClip[] enemyWindupSounds;   
    [SerializeField] private AudioClip[] deathSounds;         
    [SerializeField] private AudioClip[] uiClickSounds;       
    [SerializeField] private AudioClip[] winSounds;           
    [SerializeField] private AudioClip[] loseSounds;          

    [Header("Extra (не обязательны)")]
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioClip[] bossHitSounds;

    [Header("Variation")]
    [SerializeField] private float pitchMin = 0.92f;
    [SerializeField] private float pitchMax = 1.08f;

    public static SoundManager Instance;

    private AudioSource[] pool;
    private int next;

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

        pool = new AudioSource[10];
        for (int i = 0; i < pool.Length; i++)
        {
            var s = gameObject.AddComponent<AudioSource>();
            s.playOnAwake = false;
            s.outputAudioMixerGroup = sfxGroup;   
            pool[i] = s;
        }

        ApplySavedVolumes();
    }

    public static float ToDb(float v01)
    {
        return Mathf.Log10(Mathf.Clamp(v01, 0.0001f, 1f)) * 20f;
    }

    public void ApplySavedVolumes()
    {
        if (mixer == null) return;
        mixer.SetFloat("MasterVol", ToDb(PlayerPrefs.GetFloat("vol_master", 1f)));
        mixer.SetFloat("MusicVol", ToDb(PlayerPrefs.GetFloat("vol_music", 0.8f)));
        mixer.SetFloat("SfxVol", ToDb(PlayerPrefs.GetFloat("vol_sfx", 0.8f)));
    }

    public void SetPaused(bool paused)
    {
        if (paused) { if (pausedSnapshot != null) pausedSnapshot.TransitionTo(0.2f); }
        else { if (normalSnapshot != null) normalSnapshot.TransitionTo(0.2f); }
    }

    private void PlaySound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;

        AudioSource s = pool[next];
        next = (next + 1) % pool.Length;
        s.pitch = Random.Range(pitchMin, pitchMax);
        s.PlayOneShot(clip);
    }

    public void PlayJumpSound() { PlaySound(jumpSounds); }
    public void PlayCollectibleSound() { PlaySound(collectibleSounds); }
    public void PlayShootSound() { PlaySound(shootSounds); }
    public void PlayHurtSound() { PlaySound(hurtSounds); }
    public void PlayBossHitSound() { PlaySound(bossHitSounds); }
    public void PlayWinSound() { PlaySound(winSounds); }

    public void PlayEnemyWindupSound() { PlaySound(enemyWindupSounds); }  
    public void PlayDeathSound() { PlaySound(deathSounds); }
    public void PlayUiClickSound() { PlaySound(uiClickSounds); }
    public void PlayLoseSound() { PlaySound(loseSounds); }
}