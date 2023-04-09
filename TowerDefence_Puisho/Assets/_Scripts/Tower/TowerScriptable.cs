using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Create/Tower")]
public class TowerScriptable : ScriptableObject
{
    public TowerLevel TowerLevel;
    public bool HasAttackGroundTarget;
    public bool HasAttackAirTarget;
    public GameObject NextPrefabTower;
    public string Name;
    public float MaxHealth;
    public float MinDamageTower;
    public float MaxDamageTower;
    public float SpeedBulletTower;
    public float SpeedRotation;
    public float ReloadGunTower;
    public float RadiusAttack;
    public int PriceTower;
    public int PriceUpgrade;
}
public enum TowerLevel { Level_1, Level_2, Level_3}
