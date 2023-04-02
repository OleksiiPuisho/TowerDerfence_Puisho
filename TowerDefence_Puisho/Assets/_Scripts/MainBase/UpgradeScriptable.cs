using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade", menuName = "Create/UpgradeBase")]
public class UpgradeScriptable : ScriptableObject
{
    public Level Level;
    public float MaxHealth;
    public int Armor;
    public int Price;
}
public enum Level { Level_0, Level_1, Level_2}
