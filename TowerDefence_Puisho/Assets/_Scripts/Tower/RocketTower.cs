using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerSpace
{
    public class RocketTower : Tower
    {
        private void OnEnable()
        {
            StartCoroutine(TowerReloadCorutine());
            AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
        }
        void Update()
        {
            UpdateState(CurrentState);
        }
        public override void Attack()
        {

            if (TargetAttack != null && TargetAttack.gameObject.activeSelf)
            {
                RotateTower();
                if (_hasAttack && _lookToTarget)
                {
                    for (int i = 0; i < SpawnBullet.Length; i++)
                    {
                        SpawnBullet[i].LookAt(TargetAttack.transform);
                        var bulletObject = SpawnController.GetObject(PrefabBullet);
                        var bullet = bulletObject.GetComponent<Bullet>();
                        bulletObject.transform.SetPositionAndRotation(SpawnBullet[i].position, SpawnBullet[i].rotation);

                        bullet.DamageBullet = Random.Range(TowerScriptable.MinDamageTower, TowerScriptable.MaxDamageTower) / SpawnBullet.Length;
                        bullet.SpeedBullet = TowerScriptable.SpeedBulletTower;

                        bullet.gameObject.SetActive(true);                       
                    }
                    AudioManager.InstanceAudio.PlaySfx(SfxType.RocketTrail, _audioSource);

                    _hasAttack = false;
                    StartCoroutine(TowerReloadCorutine());

                }

                if (Vector3.Distance(transform.position, TargetAttack.transform.position) > TowerScriptable.RadiusAttack)
                {
                    TargetAttack = null;
                    _lookToTarget = false;
                    if (!_hasAttack)
                        StartCoroutine(TowerReloadCorutine());

                    UpdateState(StateTower.SearchTarget);
                }
            }
            else
                UpdateState(StateTower.SearchTarget);
        }
    }
}
