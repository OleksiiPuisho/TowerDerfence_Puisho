using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHUD : MonoBehaviour
{
    [Header("Links Main")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthSliderText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private GameObject _panelTowersBuild;
    [SerializeField] private WaveController _waveController;
    [SerializeField] private TMP_Text _notMoneyText;

    [Header("Wave UI")]
    [SerializeField] private Slider _durationToNextLevel;
    [SerializeField] private Button _startLevelButton;

    [SerializeField] private TMP_Text _levelCurrentText;
    [SerializeField] private TMP_Text _enemiesCountText;

    [SerializeField] private GameObject _waitLevelContent;
    [SerializeField] private GameObject _infoEnemyContent;    

    [Header("Selected Panel")]
    [SerializeField] private GameObject _panelActiveObject;
    [SerializeField] private Transform _contentMainBase;
    [SerializeField] private Transform _contentTower;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _levelMaxButton;
    [SerializeField] private Button _destroyButton;
    //MainBase Content
    [SerializeField] private TMP_Text _levelBase;
    [SerializeField] private TMP_Text _health;
    //Tower Content
    [SerializeField] private TMP_Text _nameTower;
    [SerializeField] private TMP_Text _levelTower;
    [SerializeField] private TMP_Text _healthTower;
    [SerializeField] private TMP_Text _damageTower;
    public static bool IsNotMoney = false;
    void Start()
    {
        //events
        SelectedObjectController.IsSelected += ObjectSelected;
        BuildPointsController.BuildingEvent += Building;
        WaveController.WaitNextLevel += WaitNextLevelHUD;
        WaveController.StartLevel += StartLevelHUD;
        //other
        _startLevelButton.onClick.AddListener(StartLevelButton);
        _destroyButton.onClick.AddListener(DestroyButton);
        _notMoneyText.gameObject.SetActive(false);
    }

    
    void Update()
    {
        _moneyText.text = GameController.Money.ToString();
        _healthSlider.maxValue = MainBase.CurrentLevel.MaxHealth;
        _healthSlider.value = MainBase.CurrentHealthBase;

        int currentHealtBase = (int)MainBase.CurrentHealthBase;
        _healthSliderText.text = currentHealtBase.ToString() + "/" + MainBase.CurrentLevel.MaxHealth.ToString();

        if(SelectedObjectController.CurrentSelectedObject == null && _panelActiveObject.activeSelf == true)
        {
            _panelActiveObject.SetActive(false);
        }
        if (_panelActiveObject.activeSelf == true)
        {
            if (SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.MainBase)
            {
                _destroyButton.gameObject.SetActive(false);
                _contentMainBase.gameObject.SetActive(true);
                _contentTower.gameObject.SetActive(false);

                _levelBase.text = MainBase.CurrentLevel.Level.ToString()[6..];
                _health.text = MainBase.CurrentHealthBase.ToString() + "/" + MainBase.CurrentLevel.MaxHealth.ToString();
                if (MainBase.CurrentLevel.Level == Level.Level_3)
                {
                    _levelMaxButton.gameObject.SetActive(true);
                }
            }
            if (SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.Tower)
            {
                _destroyButton.gameObject.SetActive(true);
                _contentMainBase.gameObject.SetActive(false);
                _contentTower.gameObject.SetActive(true);

                Tower tower = SelectedObjectController.CurrentSelectedObject.GetComponent<Tower>();
                _nameTower.text = tower.TowerScriptable.Name;
                _levelTower.text = tower.CurrentTowerLevel.ToString()[6..];
                _healthTower.text = tower.TowerScriptable.MaxHealth.ToString();
                _damageTower.text = tower.TowerScriptable.MinDamageTower.ToString() + " - " + tower.TowerScriptable.MaxDamageTower.ToString();
            }
        }
        if(BuildPointsController.ActivePoint == null && _panelTowersBuild.activeSelf == true)
        {
            _panelTowersBuild.SetActive(false);
        }
        if(_waitLevelContent.activeSelf == true)
        {
            _durationToNextLevel.value = Mathf.Lerp(_durationToNextLevel.value, WaveController.TimeToNextWave, 0.1f);
        }
        if(_infoEnemyContent.activeSelf == true)
        {
            _levelCurrentText.text = "Wave: " + WaveController.CurrentNumberWave.ToString();
            _enemiesCountText.text = "Enemies left: " + WaveController.EnemyCountLeft.ToString() + " / " + WaveController.EnemyAllCountInWave;
        }
        if(IsNotMoney)
        {
            if(_notMoneyText.gameObject.activeSelf == false)
                _notMoneyText.gameObject.SetActive(true);
            _notMoneyText.color = new(_notMoneyText.color.r, _notMoneyText.color.g, _notMoneyText.color.b, _notMoneyText.color.a - 0.3f * Time.deltaTime);
            if(_notMoneyText.color.a < 0.1f)
            {
                _notMoneyText.gameObject.SetActive(false);
                IsNotMoney = false;
            }
        }
    }
    private void DestroyButton()
    {
        var tower = SelectedObjectController.CurrentSelectedObject.GetComponent<Tower>();
        var parent = tower.transform.parent;
        parent.GetChild(0).gameObject.SetActive(true);
        parent.GetComponent<BoxCollider>().enabled = true;

        GameController.Money += tower.TowerScriptable.PriceTower / 2;

        Destroy(SelectedObjectController.CurrentSelectedObject.gameObject);
    }
    private void ObjectSelected()
    {
        _panelActiveObject.SetActive(true);
        _panelTowersBuild.SetActive(false);
        if(SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.MainBase)
        {
            _upgradeButton.onClick.AddListener(MainBase.InstanceMainBase.UpdateLevelMainBase);
        }
        if (SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.Tower)
        {
            _upgradeButton.onClick.AddListener(SelectedObjectController.CurrentSelectedObject.GetComponent<Tower>().UpgradeLevelTower);           
        }
    }
    private void Building()
    {
        _panelTowersBuild.SetActive(true);
        _panelActiveObject.SetActive(false);
    }
    private void StartLevelButton()
    {
        Time.timeScale = 10f;
    }
    private void WaitNextLevelHUD()
    {
        _waitLevelContent.SetActive(true);
        _infoEnemyContent.SetActive(false);
        _durationToNextLevel.maxValue = _waveController.TimerWave;
    }
    private void StartLevelHUD()
    {
        Time.timeScale = 1f;
        _durationToNextLevel.value = 0f;
        _waitLevelContent.SetActive(false);
        _infoEnemyContent.SetActive(true);
    }
}
