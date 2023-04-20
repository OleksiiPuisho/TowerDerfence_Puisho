using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;
public class MainBase : MonoBehaviour, IDamageble
{
    public static MainBase InstanceMainBase;
    [SerializeField] private UpgradeScriptableMainBase[] _allLevelsBase;
    public static UpgradeScriptableMainBase CurrentLevel;

    public static float CurrentHealthBase;
    public void SetDamage(float damageCount)
    {
        CurrentHealthBase -= damageCount;
        EventAggregator.Post(this, new UpdateInfoMainBaseEvent()
        {
            CurrentHealth = CurrentHealthBase,
            MaxHealthBase = CurrentLevel.MaxHealth
        });
        if (CurrentHealthBase <= 0)
        {
            EventAggregator.Post(this, new GameOverEvent());
        }
    }
    public void UpdateLevelMainBase()
    {
        for (int i = 0; i < _allLevelsBase.Length; i++)
        {
            if (CurrentLevel == _allLevelsBase[i])
            {
                int id = i + 1;
                if (_allLevelsBase[id].Price <= GameController.Money)
                {
                    CurrentLevel = _allLevelsBase[id];
                    EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -CurrentLevel.Price });
                    if (CurrentLevel.Level == Level.Level_3)
                    {
                        EventAggregator.Post(this, new SelectedMainBaseEvent()
                        {
                            Level = CurrentLevel.Level.ToString()[6..],
                            MaxHealthBase = CurrentLevel.MaxHealth.ToString()
                        });
                        EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = true });
                    }
                    else
                    {
                        EventAggregator.Post(this, new SelectedMainBaseEvent()
                        {
                            Level = CurrentLevel.Level.ToString()[6..],
                            MaxHealthBase = CurrentLevel.MaxHealth.ToString()
                        });
                    }
                }
                else
                    EventAggregator.Post(this, new NotEnoughMoneyEvent());
                return;
            }
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
