using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager I { get; private set; }

    public AudioMixerGroup musicGroup;
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public string menuSceneName = "MainMenu";
    public float fadeTime = 1f;

    AudioSource a, b, current;
    Coroutine fade;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        a = NewSource(); b = NewSource();
        current = a;
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDestroy() { if (I == this) SceneManager.sceneLoaded -= OnSceneLoaded; }

    AudioSource NewSource()
    {
        var s = gameObject.AddComponent<AudioSource>();
        s.loop = true; s.playOnAwake = false; s.volume = 0f;
        s.outputAudioMixerGroup = musicGroup;
        return s;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Play(scene.name == menuSceneName ? menuMusic : levelMusic);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;
        if (current.clip == clip && current.isPlaying) return;  

        var from = current;
        var to = (current == a) ? b : a;
        to.clip = clip; to.volume = 0f; to.Play();
        current = to;

        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(Crossfade(from, to));
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float t = 0f;
        float startFrom = from.volume;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;           
            float k = Mathf.Clamp01(t / fadeTime);
            from.volume = Mathf.Lerp(startFrom, 0f, k);
            to.volume = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }
        from.Stop();
    }
}