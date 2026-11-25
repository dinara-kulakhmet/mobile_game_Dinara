using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void OnEnable()
    {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("vol_music", 0.8f));
        sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("vol_sfx", 0.8f));
        musicSlider.onValueChanged.AddListener(SetMusic);
        sfxSlider.onValueChanged.AddListener(SetSfx);
    }

    void OnDisable()
    {
        musicSlider.onValueChanged.RemoveListener(SetMusic);
        sfxSlider.onValueChanged.RemoveListener(SetSfx);
    }

    public void SetMusic(float v)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat("vol_music", v);
        PlayerPrefs.Save();
    }

    public void SetSfx(float v)
    {
        mixer.SetFloat("SfxVol", Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat("vol_sfx", v);
        PlayerPrefs.Save();
    }
}