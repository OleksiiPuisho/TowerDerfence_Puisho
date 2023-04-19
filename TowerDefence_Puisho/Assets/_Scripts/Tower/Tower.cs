using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class Tower : MonoBehaviour
{
    [SerializeField] internal AudioSource _audioSource;
    public TowerScriptable TowerScriptable;
    public TowerLevel CurrentTowerLevel;
    internal StateTower _currentState;
    internal Enemy _targetAttack;

    [SerializeField] internal GameObject _prefabBullet;
    [SerializeField] internal Transform[] _spawnBullet;
    [SerializeField] internal Transform _turret;

    internal bool _hasAttack = false;
    internal bool _lookToTarget = false;
    void Start()
    {
        StartCoroutine(TowerReloadCorutine());
        AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
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
                SelectedObjectController.CurrentSelectedObject = newTower.GetComponent<SelectedObject>();
                var tower = newTower.GetComponent<Tower>();
                EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -TowerScriptable.PriceUpgrade});
                if (tower.CurrentTowerLevel == TowerLevel.Level_3)
                {
                    EventAggregator.Post(this, new SelectedTowerEvent()
                    {
                        Name = tower.TowerScriptable.Name,
                        MinDamage = tower.TowerScriptable.MinDamageTower.ToString(),
                        MaxDamage = tower.TowerScriptable.MaxDamageTower.ToString(),
                        Level = tower.CurrentTowerLevel.ToString()[6..],
                        PriceUpgrade = "Max Level",
                        Radius = tower.TowerScriptable.RadiusAttack.ToString(),
                        RateOfFire = tower.TowerScriptable.ReloadGunTower.ToString()
                    });
                }
                else
                {
                    EventAggregator.Post(this, new SelectedTowerEvent()
                    {
                        Name = tower.TowerScriptable.Name,
                        MinDamage = tower.TowerScriptable.MinDamageTower.ToString(),
                        MaxDamage = tower.TowerScriptable.MaxDamageTower.ToString(),
                        Level = tower.CurrentTowerLevel.ToString()[6..],
                        PriceUpgrade = tower.TowerScriptable.PriceUpgrade.ToString(),
                        Radius = tower.TowerScriptable.RadiusAttack.ToString(),
                        RateOfFire = tower.TowerScriptable.ReloadGunTower.ToString()
                    });
                }
                
                Destroy(gameObject);
            }
            else
            {
                EventAggregator.Post(this, new NotEnoughMoneyEvent());
            }
        }
        else if (CurrentTowerLevel == TowerLevel.Level_3)
        {
            EventAggregator.Post(this, new LevelMaxEvent());
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
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= TowerScriptable.RadiusAttack)
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
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= TowerScriptable.RadiusAttack)
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
                for (int i = 0; i < _spawnBullet.Length; i++)
                {
                    _spawnBullet[i].LookAt(_targetAttack.transform);
                    var bulletObject = SpawnController.GetObject(_prefabBullet);
                    var bullet = bulletObject.GetComponent<Bullet>();
                    bulletObject.transform.SetPositionAndRotation(_spawnBullet[i].position, _spawnBullet[i].rotation);

                    bullet.DamageBullet = Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower);
                    bullet.SpeedBullet = TowerScriptable.SpeedBulletTower;

                    bullet.gameObject.SetActive(true);
                    bullet.AutoPutBullet();
                    if (bullet.TypeBullet == TypeBullet.Bullet)
                        AudioManager.InstanceAudio.PlaySfx(SfxType.Bullet, _audioSource);
                    else if (bullet.TypeBullet == TypeBullet.Rocket)
                        AudioManager.InstanceAudio.PlaySfx(SfxType.RocketTrail, _audioSource);
                }

                _hasAttack = false;
                StartCoroutine(TowerReloadCorutine());
                
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
            if (Physics.Raycast(_turret.position, _turret.forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    _targetAttack = enemy;
                    _lookToTarget = true;
                }
            }
        }
    }
    internal IEnumerator TowerReloadCorutine()
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
