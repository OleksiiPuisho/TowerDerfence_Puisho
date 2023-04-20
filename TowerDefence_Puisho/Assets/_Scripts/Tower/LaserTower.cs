using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class LaserTower : Tower
{
    private void Start()
    {
        StartCoroutine(TowerReloadCorutine());
        AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
    }
    private void Update()
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
                _prefabBullet.SetActive(true);
                _spawnBullet[0].transform.LookAt(_targetAttack.transform);
                if (Physics.Raycast(_spawnBullet[0].position, _spawnBullet[0].forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
                {
                    if (hit.collider.TryGetComponent<IDamageble>(out var damageble))
                    {
                        damageble.SetDamage(Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower));
                        _hasAttack = false;
                        StartCoroutine(TowerReloadCorutine());
                    }
                }
            }

            if (Vector3.Distance(transform.position, _targetAttack.transform.position) > TowerScriptable.RadiusAttack)
            {
                _targetAttack = null;
                _lookToTarget = false;
                _prefabBullet.SetActive(false);
                UpdateState(StateTower.SearchTarget);
            }
        }
        else
        {
            _prefabBullet.SetActive(false);
            UpdateState(StateTower.SearchTarget);
        }

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
                    _prefabBullet.SetActive(true);
                }
            }
        }
    }
}
