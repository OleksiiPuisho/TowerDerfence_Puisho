using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Helpers;
using Helpers.Events;
using TowerSpace;

public class SelectedObject : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,IPointerUpHandler
{
    public GameObject SelectedIcon;
    public TypeObject TypeObject;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SelectedObjectController.CurrentSelectedObject != null)
        {
            SelectedObjectController.CurrentSelectedObject.SelectedIcon.SetActive(false);
        }
        SelectedObjectController.CurrentSelectedObject = this;
        if (TypeObject == TypeObject.MainBase)
        {
            if (MainBase.CurrentLevel.Level == Level.Level_3)
            {
                EventAggregator.Post(this, new SelectedMainBaseEvent()
                {
                    Level = MainBase.CurrentLevel.Level.ToString()[6..],
                    MaxHealthBase = MainBase.CurrentLevel.MaxHealth.ToString(),
                    PriceUpgrade = "MAX"
                });
                EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = false });
            }
            else
            {
                int id;
                for(int i = 0; i < MainBase.InstanceMainBase.AllLevelsBase.Length; i++)
                {
                    if (MainBase.CurrentLevel == MainBase.InstanceMainBase.AllLevelsBase[i])
                    {
                        id = i + 1;
                        EventAggregator.Post(this, new SelectedMainBaseEvent()
                        {
                            Level = MainBase.CurrentLevel.Level.ToString()[6..],
                            MaxHealthBase = MainBase.CurrentLevel.MaxHealth.ToString(),
                            PriceUpgrade = MainBase.InstanceMainBase.AllLevelsBase[id].Price.ToString()
                        });
                    }
                }                
            }
        }
        else
        {
            var tower = SelectedObjectController.CurrentSelectedObject.GetComponent<Tower>();           

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
                    BulletSpawnCount = tower.SpawnBullet.Length,
                    AttackType = TypeAttackString(tower)
                });
                EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = false });
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
                    BulletSpawnCount = tower.SpawnBullet.Length,
                    AttackType = TypeAttackString(tower)
                });
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
    private string TypeAttackString(Tower tower)
    {
        if (tower.TowerScriptable.HasAttackAirTarget && tower.TowerScriptable.HasAttackGroundTarget)
            return "Ground - Air";

        if (tower.TowerScriptable.HasAttackAirTarget)
            return "Air";

        if (tower.TowerScriptable.HasAttackGroundTarget)
            return "Ground";
        

        else return "Error";
    }
}
public enum TypeObject { Tower, MainBase}
