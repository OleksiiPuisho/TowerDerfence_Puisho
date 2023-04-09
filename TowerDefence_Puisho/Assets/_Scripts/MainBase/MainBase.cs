using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainBase : MonoBehaviour
{
    public static MainBase InstanceMainBase;
    [SerializeField] private UpgradeScriptableMainBase[] _allLevelsBase;
    public static UpgradeScriptableMainBase CurrentLevel;

    public static float CurrentHealthBase;

    private static event Action GameOver = delegate { };
    public void UpdateLevelMainBase()
    {
        if (CurrentLevel.Level == Level.Level_1)
        {
            if (_allLevelsBase[1].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[1];
                GameController.Money -= CurrentLevel.Price;
            }
            else
                LevelHUD.IsNotMoney = true;
        }
        else if (CurrentLevel.Level == Level.Level_2)
        {
            if (_allLevelsBase[2].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[2];
                GameController.Money -= CurrentLevel.Price;
            }
            else
                LevelHUD.IsNotMoney = true;
        }
        else if (CurrentLevel.Level == Level.Level_3)
        {
            return;
        }
        CurrentHealthBase = CurrentLevel.MaxHealth;
    }
    private void Awake()
    {       
        StartLevel();
        InstanceMainBase = this;
    }
    void Update()
    {
        if (CurrentHealthBase <= 0f)
            GameOver.Invoke();
    }
    
    private void StartLevel()
    {
        CurrentLevel = _allLevelsBase[0];
        CurrentHealthBase = CurrentLevel.MaxHealth;
    }
}
