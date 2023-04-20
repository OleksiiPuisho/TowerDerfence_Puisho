using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Helpers;
using Helpers.Events;

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
                });
                EventAggregator.Post(this, new LevelMaxEvent() { IsUpgreded = false });
            }
            else
            {
                EventAggregator.Post(this, new SelectedMainBaseEvent()
                {
                    Level = MainBase.CurrentLevel.Level.ToString()[6..],
                    MaxHealthBase = MainBase.CurrentLevel.MaxHealth.ToString(),
                });
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
                    RateOfFire = tower.TowerScriptable.ReloadGunTower.ToString()
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
                    RateOfFire = tower.TowerScriptable.ReloadGunTower.ToString()
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
}
public enum TypeObject { Tower, MainBase}
