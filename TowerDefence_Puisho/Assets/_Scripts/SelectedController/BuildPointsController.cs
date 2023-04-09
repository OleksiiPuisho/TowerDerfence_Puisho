using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BuildPointsController : MonoBehaviour
{
    public static SelectedBuilPoint ActivePoint;
    public static event Action BuildingEvent = delegate { };
    private bool _updateEvent;
    [Header("Prefabs Towers")]
    [SerializeField] private GameObject _prefabMachineGun;
    [SerializeField] private GameObject _prefabRocket;

    [Header("Buttons for Build")]
    [SerializeField] private Button _machineGunButton;
    [SerializeField] private TMP_Text _priceMachineGun;
    [SerializeField] private Button _rocketButton;
    [SerializeField] private TMP_Text _priceRocket;
    public void DeselectedBuildPoint() // and use by trigger event
    {
        if (ActivePoint != null)
        {
            ActivePoint.GetComponent<Animator>().SetBool("IsSelected", false);
            ActivePoint = null;
            _updateEvent = false;
        }
    }
    void Awake()
    {
        SelectedObjectController.IsSelected += DeselectedBuildPoint;

        _machineGunButton.onClick.AddListener(BuildMachineGun);
        _rocketButton.onClick.AddListener(BuildRocket);

        //Priced Towers
        _priceMachineGun.text = _prefabMachineGun.GetComponent<Tower>().TowerScriptable.PriceTower.ToString();
        _priceRocket.text = _prefabRocket.GetComponent<Tower>().TowerScriptable.PriceTower.ToString();
    }
    void Update()
    {
        if (ActivePoint != null && !_updateEvent)
        {
            BuildingEvent.Invoke();
            ActivePoint.GetComponent<Animator>().SetBool("IsSelected", true);
            _updateEvent = true;
        }
    }    
    private void InstantiateTower(GameObject prefab)
    {
        GameObject tower = Instantiate(prefab, ActivePoint.transform);
        tower.transform.position = ActivePoint.SpawnTower.position;
        ActivePoint.transform.GetChild(0).gameObject.SetActive(false);
        ActivePoint.GetComponent<BoxCollider>().enabled = false;
        DeselectedBuildPoint();
    }
    private void BuildMachineGun()
    {
        if (_prefabMachineGun.GetComponent<Tower>().TowerScriptable.PriceTower <= GameController.Money)
        {
            InstantiateTower(_prefabMachineGun);
        }
    }
    private void BuildRocket()
    {
        if (_prefabRocket.GetComponent<Tower>().TowerScriptable.PriceTower <= GameController.Money)
        {
            InstantiateTower(_prefabRocket);
        }
    }
}
