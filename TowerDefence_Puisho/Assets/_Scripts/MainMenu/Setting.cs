using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Setting : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _SfxVolume;

    [SerializeField] private TMP_Dropdown _graphicsQuality;
    void Awake()
    {
        _masterVolume.onValueChanged.AddListener(MasterVolume);
        _musicVolume.onValueChanged.AddListener(MusicVolume);
        _SfxVolume.onValueChanged.AddListener(SfxVolume);
        _graphicsQuality.onValueChanged.AddListener(GraphicsQuality);
    }
    private void MasterVolume(float value)
    {
        _audioMixer.SetFloat("Master", value);
    }
    private void MusicVolume(float value)
    {
        _audioMixer.SetFloat("Music", value);
    }
    private void SfxVolume(float value)
    {
        _audioMixer.SetFloat("SFX", value);
    }
    private void GraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
