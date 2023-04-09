using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveController : MonoBehaviour
{
    public static List<Enemy> EnemiesGroundList = new();
    public static List<Enemy> EnemiesAirList = new();
    private List<GameObject> _spawnEnemyList = new();
    public static event System.Action GameWin = delegate { };
    public static event System.Action WaitNextLevel = delegate { };
    public static event System.Action StartLevel = delegate { };

    [SerializeField] private Transform _spawnEnemiesGround;
    [SerializeField] private Transform _spawnEnemiesAir;

    public static int CurrentNumberWave = 1;
    public static int EnemyAllCountInWave; // всего в волне
    public static int EnemyCountLeft; //осталось в текущей волне
    private int _instantiateLeft;// осталось создать врагов

    public static float TimeToNextWave;
    public int TimerWave;

    [SerializeField] private WaveScriptable _wavesLevel;
    private Wave _currentWave;

    private bool _hasSpawn = false;
    void Awake()
    {
        StartCoroutine(TimerToNextWave());
    }
    void Update()
    {
        if(_hasSpawn && _instantiateLeft > 0)
        {
            InstantiateEnemy();
        }        
        if(EnemyCountLeft <= 0 && _hasSpawn)
        {           
            StartNextLevel();           
        }
    }
    private void StartNextLevel()
    {
        if (CurrentNumberWave - 1 >= _wavesLevel.Waves.Length)
            GameWin.Invoke();
        else
        {
            //for(int i = 0; i < _currentWave.EnemyInstances.Length; i++)
            //{
            //    for(int c = 0; c < _currentWave.EnemyInstances[i].Count; c++)
            //    {
            //        _spawnEnemyList.Add(_currentWave.EnemyInstances[c].PrefabEnemy);
            //    }
            //}
            //Debug.Log(_spawnEnemyList.Count);
            StartCoroutine(TimerToNextWave());
        }
        TimeToNextWave = 0;
        CurrentNumberWave++;
        _hasSpawn = false;        
    }
    IEnumerator TimerToNextWave()
    {
        WaitNextLevel.Invoke();
        yield return new WaitForSeconds(1f);
        TimeToNextWave++;        
        if(TimeToNextWave >= TimerWave)
        {
            StartLevel.Invoke();
            _currentWave = _wavesLevel.Waves[CurrentNumberWave - 1];
            UpdateCountEnemy();
            _hasSpawn = true;           
            StopCoroutine(TimerToNextWave());
        }
        else
            StartCoroutine(TimerToNextWave());
    }
    private void UpdateCountEnemy()
    {
        EnemyAllCountInWave = 0;
        for(int i = 0; i < _currentWave.EnemyInstances.Length; i++)
        {
            EnemyAllCountInWave += _currentWave.EnemyInstances[i].Count;
            EnemyCountLeft = EnemyAllCountInWave;
            _instantiateLeft = EnemyAllCountInWave;
        }
    }
    private void InstantiateEnemy()
    {
        GameObject enemy = Instantiate(_currentWave.EnemyInstances[Random.Range(0, _currentWave.EnemyInstances.Length)].PrefabEnemy);
        _instantiateLeft--;
        if (enemy.GetComponent<Enemy>().EnemyScriptable.TypeEnemy == TypeEnemy.Ground)
        enemy.transform.position = _spawnEnemiesGround.position;
        else
            enemy.transform.position = _spawnEnemiesAir.position;
        
        _hasSpawn = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _hasSpawn = true;
        }
    }
}
