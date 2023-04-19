using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    BuildTower,
    RocketTrail,
    Bullet,
    DestroyEnemy
}
public enum MusicType
{
    MainMenu,
    BackgroundMusic
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<SfxData> _sfxDatas = new();
    [SerializeField] private List<MusicData> _musicDatas = new();

    public static AudioManager InstanceAudio;
    public void PlaySfx(SfxType sfxType, AudioSource audioSource)
    {
        var sfxData = _sfxDatas.Find(SfxData => SfxData.TypeSfx == sfxType);
        audioSource.PlayOneShot(sfxData.Clip);
    }
    public void PlaySfxWalking(SfxType sfxType, AudioSource audioSource)
    {
        var sfxData = _sfxDatas.Find(SfxData => SfxData.TypeSfx == sfxType);
        audioSource.clip = sfxData.Clip;
        audioSource.Play();
    }
    public void PlayMusic(MusicType musicType, AudioSource audioSource)
    {
        var musicData = _musicDatas.Find(MusicData => MusicData.TypeMusic == musicType);
        audioSource.clip = musicData.Clip;
        audioSource.Play();

    }
    private void Awake()
    {
        if(InstanceAudio == null)
        {
            InstanceAudio = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
[System.Serializable]
public class SfxData
{
    [SerializeField] private SfxType _sfxType;
    [SerializeField] private AudioClip _clip;

    public SfxType TypeSfx => _sfxType;
    public AudioClip Clip => _clip;
}
[System.Serializable]
public class MusicData
{
    [SerializeField] private MusicType _musicType;
    [SerializeField] private AudioClip _clip;

    public MusicType TypeMusic => _musicType;
    public AudioClip Clip => _clip;
}
