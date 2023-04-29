using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Create/Wave")]
public class WaveScriptable : ScriptableObject
{
    public Wave[] Waves;
}
[System.Serializable]
public class EnemyInstance
{
    public int Count;
    public GameObject PrefabEnemy;
}
[System.Serializable]
public class Wave
{
    public EnemyInstance[] EnemyInstances;
    public int RewardWave;
}
