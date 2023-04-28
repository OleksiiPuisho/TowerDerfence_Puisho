using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;
public class WaveController : MonoBehaviour
{
    public static List<Enemy> EnemiesGroundList = new();
    public static List<Enemy> EnemiesAirList = new();
    private List<GameObject> _currentSpawnEnemyList = new();// осталось создать врагов
    private int _enemyCounter;

    [SerializeField] private Transform _spawnEnemiesGround;
    [SerializeField] private Transform _spawnEnemiesAir;

    [SerializeField] private WaveScriptable _wavesLevel;

    [SerializeField] private int _timeToWave;
    private int _timeToWaveHelper;
    private int _currentWave = 0;
    private bool _hasSpawn = false;
    void Awake()
    {
        StartCoroutine(TimerToNextWave());
        EventAggregator.Subscribe<EnemyDeathEvent>(EnemyDeathChange);
    }
    void Update()
    {
        if (_hasSpawn == true)
            SpawnEnemy();
    }
    IEnumerator TimerToNextWave()
    {
        yield return new WaitForSeconds(1f);
        _timeToWaveHelper++;
        EventAggregator.Post(this, new WaitWaweEvent() { Timer = _timeToWaveHelper, MaxTime = _timeToWave });
        if (_timeToWaveHelper >= _timeToWave)
        {
            UpdateListEnemy();
            _currentWave++;
            StopCoroutine(TimerToNextWave());
            EventAggregator.Post(this, new StartWaveEvent() 
            { 
                CurrentWaveCount = _currentWave, 
                AllWave = _wavesLevel.Waves.Length, 
                CountEnemyInWaveStart = _currentSpawnEnemyList.Count
            });
            
            _hasSpawn = true;
        }
        else
        StartCoroutine(TimerToNextWave());
    }
    private void UpdateListEnemy()
    {
        _currentSpawnEnemyList.Clear();
        for (int i = 0; i < _wavesLevel.Waves[_currentWave].EnemyInstances.Length; i++)
        {
            for (int c = 0; c < _wavesLevel.Waves[_currentWave].EnemyInstances[i].Count; c++)
            {
                _currentSpawnEnemyList.Add(_wavesLevel.Waves[_currentWave].EnemyInstances[i].PrefabEnemy);
            }
        }
        _enemyCounter = _currentSpawnEnemyList.Count;
    }
    private void SpawnEnemy()
    {
        if(_currentSpawnEnemyList.Count > 0 && _hasSpawn)
        {
            var enemy = _currentSpawnEnemyList[Random.Range(0, _currentSpawnEnemyList.Count)];
            var enemyObject = SpawnController.GetObject(enemy);

            if (enemyObject.GetComponent<Enemy>().EnemyScriptable.TypeEnemy == TypeEnemy.Ground)
            {
                enemyObject.transform.SetPositionAndRotation(_spawnEnemiesGround.position, _spawnEnemiesGround.rotation);
                EnemiesGroundList.Add(enemyObject.GetComponent<Enemy>());
            }
            else
            {
                enemyObject.transform.SetPositionAndRotation(_spawnEnemiesAir.position, _spawnEnemiesAir.rotation);
                EnemiesAirList.Add(enemyObject.GetComponent<Enemy>());
            }
            _currentSpawnEnemyList.Remove(enemy);
            enemyObject.SetActive(true);
            _hasSpawn = false;
            StartCoroutine(TimeToSpawn());
        }
    }
    private void EnemyDeathChange(object sender, EnemyDeathEvent eventData)
    {
        _enemyCounter--;
        if (_enemyCounter <= 0)
        {
            if (_currentWave >= _wavesLevel.Waves.Length)
            {
                EventAggregator.Post(this, new GameWinEvent());
            }
            else
            {
                _timeToWaveHelper = 0;
                EventAggregator.Post(this, new WaitWaweEvent() { Timer = _timeToWaveHelper, MaxTime = _timeToWave });
                StartCoroutine(TimerToNextWave());
            }
        }
        else
        {
            EventAggregator.Post(this, new UpdateInfoWaveEvent() { EnemiesLeft = _enemyCounter });
        }
    }
    IEnumerator TimeToSpawn()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        _hasSpawn = true;
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<EnemyDeathEvent>(EnemyDeathChange);
    }
}
