using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

namespace TowerSpace
{
    public class LaserTower : Tower
    {
        public override void Attack()
        {

            if (TargetAttack != null && TargetAttack.gameObject.activeSelf)
            {
                RotateTower();
                if (_hasAttack && _lookToTarget)
                {
                    SpawnBullet[0].transform.LookAt(TargetAttack.transform);
                    if (Physics.Raycast(SpawnBullet[0].position, SpawnBullet[0].forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
                    {
                        if (hit.collider.TryGetComponent<IDamageble>(out var damageble))
                        {
                            damageble.SetDamage(Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower));
                            _hasAttack = false;
                            StartCoroutine(TowerReloadCorutine());
                        }
                        else
                        {
                            PrefabBullet.SetActive(false);
                        }
                    }
                }

                if (Vector3.Distance(transform.position, TargetAttack.transform.position) > TowerScriptable.RadiusAttack)
                {
                    TargetAttack = null;
                    _lookToTarget = false;
                    PrefabBullet.SetActive(false);
                    UpdateState(StateTower.SearchTarget);
                }
            }
            else
            {
                PrefabBullet.SetActive(false);
                UpdateState(StateTower.SearchTarget);
            }

        }
        public override void RotateTower()
        {
            var directionRotatation = Quaternion.LookRotation(TargetAttack.transform.position - Turret.position);
            Turret.rotation = Quaternion.Slerp(Turret.rotation, directionRotatation, TowerScriptable.SpeedRotation * Time.deltaTime);

            if (Physics.Raycast(Turret.position, Turret.forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    TargetAttack = enemy;
                    _lookToTarget = true;
                    PrefabBullet.SetActive(true);
                }
            }
            else
            {
                PrefabBullet.SetActive(false);
                _lookToTarget = false;
            }
        }
        private void OnEnable()
        {
            StartCoroutine(TowerReloadCorutine());
            AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
        }
        private void Update()
        {
            UpdateState(CurrentState);
        }
    }
}
