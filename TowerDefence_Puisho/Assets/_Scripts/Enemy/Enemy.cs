using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Movement,
    SearchTarget,
    AttackTower,
    AttackMainBase
}
public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private Transform _mainBaseTarget;
    [SerializeField] private Transform _targetAttack;
    private bool hasAttack;
    [SerializeField] private EnemyScriptable _enemyScriptable;
    
    private EnemyState _currentEnemyState;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(ReloadCorutine());
        _currentEnemyState = EnemyState.Movement;
    }

    void Update()
    {
        StateAgent(_currentEnemyState);
        Debug.Log(_currentEnemyState);
    }
    private void StateAgent(EnemyState enemyState)
    {
        _currentEnemyState = enemyState;
        switch (enemyState)
        {
            case EnemyState.Movement:
                Movement();
                break;
            case EnemyState.SearchTarget:
                SearchTarget();
                break;
            case EnemyState.AttackTower:
                AttackTower();
                break;
            case EnemyState.AttackMainBase:
                AttackMainBase();
                break;            
        }
    }
    private void Movement()
    {
        if (_currentEnemyState == EnemyState.Movement)
            _agent.SetDestination(_mainBaseTarget.position);

        if (Vector3.Distance(_agent.transform.position, _mainBaseTarget.position) <= _enemyScriptable.RadiusShooting)
        {
            StateAgent(EnemyState.AttackMainBase);
        }
        else
        {
            if (hasAttack)
            {
                if(_targetAttack == null)
                    StateAgent(EnemyState.SearchTarget);
                else
                {
                    Shooting(_targetAttack);
                }
            }
        }
    }
    private void SearchTarget()
    {
        foreach(Tower tower in TowerController.Towers)
        {
            if(Vector3.Distance(_agent.transform.position, tower.transform.position) <= _enemyScriptable.RadiusShooting)
            {
                _targetAttack = tower.transform;
                StateAgent(EnemyState.AttackTower);
            }
        }
    }
    private void AttackTower()
    {
        if (hasAttack)
        {
            Shooting(_targetAttack);
        }
    }
    private void AttackMainBase()
    {
        if (hasAttack)
        {
            _agent.destination = transform.position;
            _targetAttack = _mainBaseTarget;
            Shooting(_targetAttack);
        }
    }
    private void Shooting(Transform target)
    {
        if(_currentEnemyState == EnemyState.AttackTower)
        {
            Instantiate(_bulletPrefab, _spawnBullet);
            hasAttack = false;
            StartCoroutine(ReloadCorutine());
            StateAgent(EnemyState.Movement);
        }
        if(_currentEnemyState == EnemyState.AttackMainBase)
        {
            ////shoot
            Debug.Log("MainBase");
            hasAttack = false;
            StartCoroutine(ReloadCorutine());
        }
    }
    IEnumerator ReloadCorutine()
    {
        yield return new WaitForSeconds(_enemyScriptable.ReloadGun);
        hasAttack = true;
    }
}