using Helpers;
using Helpers.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private AudioSource _audioSource;
    public static int Money;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        EventAggregator.Subscribe<MoneyUpdateEvent>(MoneyUpdateChange);
        EventAggregator.Subscribe<StartGameEvent>(StartGameChange);
        
    }
    private void Start()
    {
        AudioManager.InstanceAudio.PlayMusic(MusicType.BackgroundMusic, _audioSource);
    }
    private void MoneyUpdateChange(object sender, MoneyUpdateEvent eventData)
    {
        Money += eventData.MoneyCount;
        EventAggregator.Post(this, new MoneyUpdateUIEvent() { Count = Money });
    }
    private void StartGameChange(object sender, StartGameEvent eventData)
    {
        Money = eventData.StartMoney;
        EventAggregator.Post(this, new MoneyUpdateUIEvent() { Count = Money});
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<MoneyUpdateEvent>(MoneyUpdateChange);
        EventAggregator.Unsubscribe<StartGameEvent>(StartGameChange);
    }
}
