using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;
using Helpers.Events;
using TMPro;

public class BuildPointsController : MonoBehaviour
{
    public static SelectedBuilPoint ActivePoint;
    [Header("Prefabs Towers")]
    [SerializeField] private GameObject _prefabMachineGun;
    [SerializeField] private GameObject _prefabRocket;
    [SerializeField] private GameObject _prefabLaser;

    [Header("Buttons for Build")]
    [SerializeField] private Button _machineGunButton;
    [SerializeField] private TMP_Text _priceMachineGun;
    [SerializeField] private Button _rocketButton;
    [SerializeField] private TMP_Text _priceRocket;
    [SerializeField] private Button _laserButton;
    [SerializeField] private TMP_Text _priceLaser;
    public void DeselectedBuildPoint()
    {
        if (ActivePoint != null)
        {
            ActivePoint.GetComponent<Animator>().SetBool("IsSelected", false);
            ActivePoint = null;
        }
        EventAggregator.Post(this, new DeselectedAllEvent());
    }
    void Awake()
    {
        _priceMachineGun.text = _prefabMachineGun.GetComponent<Tower>().TowerScriptable.PriceTower.ToString();
        _priceRocket.text = _prefabRocket.GetComponent<Tower>().TowerScriptable.PriceTower.ToString();
        _priceLaser.text = _prefabLaser.GetComponent<LaserTower>().TowerScriptable.PriceTower.ToString();

        EventAggregator.Subscribe<SelectedBuildPointEvent>(SelectedPoint);
        EventAggregator.Subscribe<DeselectedAllEvent>(Deselected);
        _machineGunButton.onClick.AddListener(BuildMachineGun);
        _rocketButton.onClick.AddListener(BuildRocket);
        _laserButton.onClick.AddListener(BuildLaser);
    } 
    private void SelectedPoint(object sender, SelectedBuildPointEvent eventData)
    {
        ActivePoint.GetComponent<Animator>().SetBool("IsSelected", true);
    }
    private GameObject InstantiateTower(GameObject prefab)
    {
        GameObject tower = SpawnController.GetObject(prefab);
        tower.transform.SetParent(ActivePoint.transform);

        tower.transform.position = ActivePoint.SpawnTower.position;
        ActivePoint.transform.GetChild(0).gameObject.SetActive(false);
        ActivePoint.GetComponent<BoxCollider>().enabled = false;
        DeselectedBuildPoint();
        return tower;
    }
    private void BuildMachineGun()
    {
        if (_prefabMachineGun.GetComponent<Tower>().TowerScriptable.PriceTower <= GameController.Money)
        {
            var obj =  InstantiateTower(_prefabMachineGun);
            EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -obj.GetComponent<Tower>().TowerScriptable.PriceTower });           
        }
    }
    private void BuildRocket()
    {
        if (_prefabRocket.GetComponent<Tower>().TowerScriptable.PriceTower <= GameController.Money)
        {
            var obj = InstantiateTower(_prefabRocket);
            EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -obj.GetComponent<Tower>().TowerScriptable.PriceTower });
        }
    }
    private void BuildLaser()
    {
        if (_prefabLaser.GetComponent<LaserTower>().TowerScriptable.PriceTower <= GameController.Money)
        {
            var obj = InstantiateTower(_prefabLaser);
            EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = -obj.GetComponent<LaserTower>().TowerScriptable.PriceTower });
        }
    }
    private void Deselected(object sender, DeselectedAllEvent eventData)
    {
        if (ActivePoint != null)
        {
            ActivePoint.GetComponent<Animator>().SetBool("IsSelected", false);
            ActivePoint = null;
        }
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<SelectedBuildPointEvent>(SelectedPoint);
    }
}
