using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainBase : MonoBehaviour
{
    [SerializeField] private UpgradeScriptable[] _allLevelsBase;
    public static UpgradeScriptable CurrentLevel;

    public static float MaxHealth;
    public static float CurrentHealth;
    public static int Armor;

    private static event Action GameOver = delegate { };
    private void Awake()
    {       
        StartLevel();
    }
    void Update()
    {
        if (CurrentHealth <= 0f)
            GameOver.Invoke();
    }
    public void UpdateLevelMainBase()
    {
        if (CurrentLevel.Level == Level.Level_0)
        {
            if (_allLevelsBase[1].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[1];
                GameController.Money -= CurrentLevel.Price;
            }
        }
        else if(CurrentLevel.Level == Level.Level_1)
        {
            if (_allLevelsBase[2].Price <= GameController.Money)
            {
                CurrentLevel = _allLevelsBase[2];
                GameController.Money -= CurrentLevel.Price;
            }
        }
        else if (CurrentLevel.Level == Level.Level_2)
        {
            return;
        }
        MaxHealth = CurrentLevel.MaxHealth;
        CurrentHealth = MaxHealth;
        Armor = CurrentLevel.Armor;
    }
    private void StartLevel()
    {
        CurrentLevel = _allLevelsBase[0];
        MaxHealth = CurrentLevel.MaxHealth;
        CurrentHealth = MaxHealth;
        Armor = CurrentLevel.Armor;
    }
}
