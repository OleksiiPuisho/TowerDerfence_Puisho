using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create/Enemy")]
public class EnemyScriptable : ScriptableObject
{
    public float HealthEnemy;
    public float MinDamage;
    public float MaxDamage;
    public float ReloadGun;
    public float RadiusShooting;
    public int Reward;
}
