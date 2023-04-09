using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create/Enemy")]
public class EnemyScriptable : ScriptableObject
{
    public TypeEnemy TypeEnemy;
    public float MaxHealthEnemy;
    public float MinDamage;
    public float MaxDamage;
    public float SpeedBulletEneny;

    public float ReloadGun;
    public float RadiusShooting;
    public int Reward;
}
public enum TypeEnemy { Ground, Air}
