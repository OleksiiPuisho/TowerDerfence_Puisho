using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : Tower
{
    private void Start()
    {
        StartCoroutine(TowerReloadCorutine());
        AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
    }
    void Update()
    {
        UpdateState(_currentState);
    }
    private void UpdateState(StateTower stateTower)
    {
        _currentState = stateTower;
        switch (stateTower)
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
        if (TowerScriptable.HasAttackAirTarget)
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
                    var bullet = bulletObject.GetComponent<RocketBehavior>();
                    bulletObject.transform.SetPositionAndRotation(_spawnBullet[i].position, _spawnBullet[i].rotation);

                    bullet.DamageBullet = Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower);
                    bullet.SpeedBullet = TowerScriptable.SpeedBulletTower;

                    bullet.gameObject.SetActive(true);

                    bullet.AutoPutBullet();
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
}
