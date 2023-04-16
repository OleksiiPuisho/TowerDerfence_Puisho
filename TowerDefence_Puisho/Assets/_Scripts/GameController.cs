using Helpers;
using Helpers.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int Money;
    void Awake()
    {
        EventAggregator.Subscribe<MoneyUpdateEvent>(MoneyUpdateChange);
        EventAggregator.Subscribe<StartGameEvent>(StartGameChange);
    }

    
    void Update()
    {
        
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
}
