using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

namespace TowerSpace
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] internal AudioSource _audioSource;
        public TowerScriptable TowerScriptable;
        public TowerLevel CurrentTowerLevel;
        protected StateTower CurrentState;
        protected Enemy TargetAttack;

        [SerializeField] protected GameObject PrefabBullet;
        [SerializeField] internal Transform[] SpawnBullet;
        [SerializeField] protected Transform Turret;

        protected bool _hasAttack = false;
        protected bool _lookToTarget = false;
        public void UpgradeLevelTower()
        {
            if (CurrentTowerLevel != TowerLevel.Level_3)
            {
                if (TowerScriptable.PriceUpgrade <= GameController.Money)
                {
                    GameObject newTower = Instantiate(TowerScriptable.NextPrefabTower, transform.parent);
                    newTower.transform.position = transform.position;
                    SelectedObjectController.CurrentSelectedObject = newTower.GetComponent<SelectedObject>();
                    var tower = newTower.GetComponent<Tower>();
                    EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -TowerScriptable.PriceUpgrade });
                    if (tower.CurrentTowerLevel == TowerLevel.Level_3)
                    {
                        EventAggregator.Post(this, new SelectedTowerEvent()
                        {
                            Name = tower.TowerScriptable.Name,
                            MinDamage = tower.TowerScriptable.MinDamageTower,
                            MaxDamage = tower.TowerScriptable.MaxDamageTower,
                            Level = tower.CurrentTowerLevel.ToString()[6..],
                            PriceUpgrade = "Max Level",
                            Radius = tower.TowerScriptable.RadiusAttack.ToString(),
                            BulletSpawnCount = tower.SpawnBullet.Length
                        });
                        EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = true });
                    }
                    else
                    {
                        EventAggregator.Post(this, new SelectedTowerEvent()
                        {
                            Name = tower.TowerScriptable.Name,
                            MinDamage = tower.TowerScriptable.MinDamageTower,
                            MaxDamage = tower.TowerScriptable.MaxDamageTower,
                            Level = tower.CurrentTowerLevel.ToString()[6..],
                            PriceUpgrade = tower.TowerScriptable.PriceUpgrade.ToString(),
                            Radius = tower.TowerScriptable.RadiusAttack.ToString(),
                            BulletSpawnCount = tower.SpawnBullet.Length
                        });
                    }

                    Destroy(gameObject);
                }
                else
                {
                    EventAggregator.Post(this, new NotEnoughMoneyEvent());
                }
            }
        }
        public virtual void UpdateState(StateTower stateTower)
        {
            CurrentState = stateTower;
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
        public virtual void SearchTarget()
        {
            if (TowerScriptable.HasAttackGroundTarget)
            {
                if (WaveController.EnemiesGroundList.Count > 0)
                {
                    foreach (Enemy enemy in WaveController.EnemiesGroundList)
                    {
                        if (Vector3.Distance(transform.position, enemy.transform.position) <= TowerScriptable.RadiusAttack)
                        {
                            TargetAttack = enemy;
                            UpdateState(StateTower.Attack);
                            return;
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
                            TargetAttack = enemy;
                            UpdateState(StateTower.Attack);
                            return;
                        }
                    }
                }
            }
        }
        public virtual void Attack()
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
                    AudioManager.InstanceAudio.PlaySfx(SfxType.Bullet, _audioSource);

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
        public virtual void RotateTower()
        {
            var directionRotatation = Quaternion.LookRotation(TargetAttack.transform.position - Turret.position);
            Turret.rotation = Quaternion.Slerp(Turret.rotation, directionRotatation, TowerScriptable.SpeedRotation * Time.deltaTime);
            if (Physics.Raycast(SpawnBullet[0].position, SpawnBullet[0].forward, out RaycastHit hit, TowerScriptable.RadiusAttack))
            {
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    TargetAttack = enemy;
                    _lookToTarget = true;
                }
            }
            else _lookToTarget = false;
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
}
