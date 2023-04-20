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
                        MinDamage = tower.TowerScriptable.MinDamageTower.ToString(),
                        MaxDamage = tower.TowerScriptable.MaxDamageTower.ToString(),
                        Level = tower.CurrentTowerLevel.ToString()[6..],
                        PriceUpgrade = "Max Level",
                        Radius = tower.TowerScriptable.RadiusAttack.ToString(),
                        RateOfFire = tower.TowerScriptable.ReloadGunTower.ToString()
                    });
                    EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = true});
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
