using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerScriptable TowerScriptable;
    public TowerLevel CurrentTowerLevel;
    public float CurrentHealthTower;
    private StateTower _currentState;
    private Enemy _targetAttack;

    [SerializeField] private GameObject _prefabBullet;
    [SerializeField] private GameObject _prefabParticleShot;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private Transform _turret;

    private bool _hasAttack = false;
    private bool _lookToTarget = false;
    void Start()
    {
        TowerController.Towers.Add(this);
        StartCoroutine(TowerReloadCorutine());
        CurrentHealthTower = TowerScriptable.MaxHealth;
    }

    void Update()
    {
        UpdateState(_currentState);
        
    }
    public void UpgradeLevelTower()
    {
        if(CurrentTowerLevel == TowerLevel.Level_1 || CurrentTowerLevel == TowerLevel.Level_2)
        {
            if (TowerScriptable.PriceUpgrade <= GameController.Money)
            {
                GameObject newTower = Instantiate(TowerScriptable.NextPrefabTower, transform.parent);
                newTower.transform.position = transform.position;
                Destroy(gameObject);
            }
            else
            {
                LevelHUD.IsNotMoney = true;
            }
        }
        else if (CurrentTowerLevel == TowerLevel.Level_3)
        {
            Debug.Log("Level Max");
            return;
        }
    }
    private void UpdateState(StateTower stateTower)
    {
        _currentState = stateTower;
        switch(stateTower)
        {
            case StateTower.SearchTarget:
                SearchTarget();
                break;
            case StateTower.Attack:
                Attack();
                break;
        }
    }
    private void SearchTarget()
    {
        if (TowerScriptable.HasAttackGroundTarget)
        {
            if (WaveController.EnemiesGroundList.Count > 0)
            {
                foreach (Enemy enemy in WaveController.EnemiesGroundList)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < TowerScriptable.RadiusAttack)
                    {
                        _targetAttack = enemy;
                        UpdateState(StateTower.Attack);
                    }
                }
            }
        }
        if(TowerScriptable.HasAttackAirTarget)
        {
            if (WaveController.EnemiesAirList.Count > 0)
            {
                foreach (Enemy enemy in WaveController.EnemiesAirList)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < TowerScriptable.RadiusAttack)
                    {
                        _targetAttack = enemy;
                        UpdateState(StateTower.Attack);
                    }
                }
            }
        }
    }
    private void Attack()
    {
        if (_targetAttack != null)
        {
            RotateTower();
            if (_hasAttack && _lookToTarget)
            {
                GameObject bullet = Instantiate(_prefabBullet);
                bullet.transform.position = _spawnBullet.position;
                bullet.transform.LookAt(_targetAttack.transform);
                bullet.GetComponent<Bullet>().DamageBullet = Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower);
                bullet.GetComponent<Bullet>().SpeedBullet = TowerScriptable.SpeedBulletTower;

                GameObject particle = Instantiate(_prefabParticleShot);
                particle.transform.position = _spawnBullet.position;

                StartCoroutine(TowerReloadCorutine());
                _hasAttack = false;
            }
            if (Vector3.Distance(transform.position, _targetAttack.transform.position) > TowerScriptable.RadiusAttack)
            {
                _targetAttack = null;
                _lookToTarget = false;
                if (!_hasAttack)
                    StartCoroutine(TowerReloadCorutine());

                UpdateState(StateTower.SearchTarget);
            }
        }
        else
            UpdateState(StateTower.SearchTarget);
    }
    private void RotateTower()
    {
        var directionRotatation = Quaternion.LookRotation(_targetAttack.transform.position - _turret.position);
        _turret.rotation = Quaternion.Slerp(_turret.rotation, directionRotatation, TowerScriptable.SpeedRotation * Time.deltaTime);
        if (!_lookToTarget)
        {
            if (Physics.Raycast(_spawnBullet.position, _spawnBullet.forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    _targetAttack = enemy;
                    _lookToTarget = true;
                }
            }
            else
                UpdateState(StateTower.SearchTarget);
        }
    }
    private void OnDestroy()
    {
        TowerController.Towers.Remove(this);
    }
    private void OnEnable()
    {
        GameController.Money -= TowerScriptable.PriceTower;
    }
    IEnumerator TowerReloadCorutine()
    {
        yield return new WaitForSeconds(TowerScriptable.ReloadGunTower);
        _hasAttack = true;
    }
}
public enum StateTower
{
    SearchTarget,
    Attack
}
