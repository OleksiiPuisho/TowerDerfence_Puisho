using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MainBase : MonoBehaviour
{
    [SerializeField] private Button _upGradeButton;

    [SerializeField] private UpgradeScriptable[] _allLevelsBase;
    private UpgradeScriptable _currentLevel;
    private float _maxHealth;
    private float _currentHealth;
    private int _armor;

    private static event Action GameOver = delegate { };
    private void Awake()
    {       
        _upGradeButton.onClick.AddListener(UpdateLevel);
        StartLevel();
    }
    void Update()
    {
        if (_currentHealth <= 0f)
            GameOver.Invoke();
    }
    private void UpdateLevel()// add     if(money >= price)
    {
        if (_currentLevel.LevelUp == Level.Level_0)
        {
            _currentLevel = _allLevelsBase[1];
        }
        else if(_currentLevel.LevelUp == Level.Level_1)
        {
            _currentLevel = _allLevelsBase[2];
        }
        _maxHealth = _currentLevel.MaxHealth;
        _currentHealth = _maxHealth;
        _armor = _currentLevel.Armor;
        Debug.Log(_currentHealth+" / " + _maxHealth + "  /  " + _armor);
    }
    private void StartLevel()
    {
        _currentLevel = _allLevelsBase[0];
        _maxHealth = _currentLevel.MaxHealth;
        _currentHealth = _maxHealth;
        _armor = _currentLevel.Armor;
    }
}
