using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade", menuName = "Create/UpgradeBase")]
public class UpgradeScriptableMainBase : ScriptableObject
{
    public Level Level;
    public float MaxHealth;
    public int Price;
}
public enum Level { Level_1, Level_2, Level_3}
