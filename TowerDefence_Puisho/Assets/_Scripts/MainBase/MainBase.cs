using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;
public class MainBase : MonoBehaviour
{
    public static MainBase InstanceMainBase;
    [SerializeField] private UpgradeScriptableMainBase[] _allLevelsBase;
    public static UpgradeScriptableMainBase CurrentLevel;

    public static float CurrentHealthBase;
    public void UpdateLevelMainBase()
    {
        if (CurrentLevel.Level == Level.Level_1)
        {
            if (_allLevelsBase[1].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[1];
                EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = - CurrentLevel.Price});
                EventAggregator.Post(this, new SelectedMainBaseEvent() { Level = CurrentLevel.Level.ToString()[6..], MaxHealthBase = CurrentLevel.MaxHealth.ToString()});
            }
            else
                EventAggregator.Post(this, new NotEnoughMoneyEvent());
        }
        else if (CurrentLevel.Level == Level.Level_2)
        {
            if (_allLevelsBase[2].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[2];
                EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -CurrentLevel.Price });
                EventAggregator.Post(this, new SelectedMainBaseEvent() { Level = CurrentLevel.Level.ToString()[6..], MaxHealthBase = CurrentLevel.MaxHealth.ToString()});
            }
            else
                EventAggregator.Post(this, new NotEnoughMoneyEvent());
        }
        else if (CurrentLevel.Level == Level.Level_3)
        {
            EventAggregator.Post(this, new LevelMaxEvent());
        }
        CurrentHealthBase = CurrentLevel.MaxHealth;
    }
    private void Awake()
    {       
        InstanceMainBase = this;
        EventAggregator.Subscribe<StartGameEvent>(StartGameChange);
    }
    void Update()
    {
        if (CurrentHealthBase <= 0f)
            EventAggregator.Post(this, new GameOverEvent());
    }
    
    private void StartGameChange(object sender, StartGameEvent eventData)
    {
        CurrentLevel = _allLevelsBase[0];
        CurrentHealthBase = CurrentLevel.MaxHealth;
        EventAggregator.Post(this, new UpdateInfoMainBaseEvent()
        {
            MaxHealthBase = CurrentLevel.MaxHealth,
            CurrentHealth = CurrentHealthBase
        });
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<StartGameEvent>(StartGameChange);
    }
}
