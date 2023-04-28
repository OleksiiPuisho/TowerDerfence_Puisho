using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;
public class MainBase : MonoBehaviour, IDamageble
{
    [SerializeField] private ParticleSystem _gameOverParticle;
    [SerializeField] private ParticleSystem _gameWinParticle;
    public static MainBase InstanceMainBase;
    public UpgradeScriptableMainBase[] AllLevelsBase;
    public static UpgradeScriptableMainBase CurrentLevel;

    public static float CurrentHealthBase;
    public void SetDamage(float damageCount)
    {
        CurrentHealthBase -= damageCount;
        EventAggregator.Post(this, new UpdateInfoMainBaseEvent()
        {
            CurrentHealth = CurrentHealthBase,
            MaxHealthBase = CurrentLevel.MaxHealth,
            RepairPrice = (CurrentLevel.MaxHealth - CurrentHealthBase) * 2
        });
        if (CurrentHealthBase <= 0)
        {
            _gameOverParticle.gameObject.SetActive(true);
            EventAggregator.Post(this, new GameOverEvent());
            GetComponent<SelectedObject>().enabled = false;
        }
    }
    public void UpdateLevelMainBase()
    {
        for (int i = 0; i < AllLevelsBase.Length; i++)
        {
            if (CurrentLevel == AllLevelsBase[i])
            {
                int id = i + 1;
                if (AllLevelsBase[id].Price <= GameController.Money)
                {
                    CurrentLevel = AllLevelsBase[id];
                    EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -CurrentLevel.Price });
                    CurrentHealthBase = CurrentLevel.MaxHealth;
                    EventAggregator.Post(this, new UpdateInfoMainBaseEvent()
                    {
                        MaxHealthBase = CurrentLevel.MaxHealth,
                        CurrentHealth = CurrentHealthBase
                    });
                    EventAggregator.Post(this, new SelectedMainBaseEvent()
                    {
                        Level = CurrentLevel.Level.ToString()[6..],
                        MaxHealthBase = CurrentLevel.MaxHealth.ToString(),
                        PriceUpgrade = AllLevelsBase[id + 1].Price.ToString()
                    });
                }
                else
                {
                    EventAggregator.Post(this, new NotEnoughMoneyEvent());
                }
                return;
            }
        }
    }
    private void Awake()
    {       
        InstanceMainBase = this;
        EventAggregator.Subscribe<StartGameEvent>(StartGameChange);
        EventAggregator.Subscribe<GameWinEvent>(GameWinChange);
    }
    
    private void StartGameChange(object sender, StartGameEvent eventData)
    {
        CurrentLevel = AllLevelsBase[0];
        CurrentHealthBase = CurrentLevel.MaxHealth;
        EventAggregator.Post(this, new UpdateInfoMainBaseEvent()
        {
            MaxHealthBase = CurrentLevel.MaxHealth,
            CurrentHealth = CurrentHealthBase
        });
    }
    private void GameWinChange(object sender, GameWinEvent eventData)
    {
        _gameWinParticle.gameObject.SetActive(true);
        GetComponent<SelectedObject>().enabled = false;
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<StartGameEvent>(StartGameChange);
        EventAggregator.Unsubscribe<GameWinEvent>(GameWinChange);
    }
}
