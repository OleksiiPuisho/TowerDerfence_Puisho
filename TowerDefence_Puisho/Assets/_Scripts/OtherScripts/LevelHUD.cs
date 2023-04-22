using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Helpers;
using Helpers.Events;
using UnityEngine.SceneManagement;

public class LevelHUD : MonoBehaviour
{
    [Header("Links Main")]
    [SerializeField] private Canvas _canvasHUD;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthSliderText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private GameObject _panelTowersBuild;
    [SerializeField] private TMP_Text _notMoneyText;
    [SerializeField] private TMP_Text _levelMaxText;

    [Header("Wave UI")]
    [SerializeField] private Slider _durationToNextLevel;
    [SerializeField] private Button _startLevelButton;
    [SerializeField] private int _timeScale;

    [SerializeField] private TMP_Text _levelCurrentText;
    [SerializeField] private TMP_Text _enemiesCountText;

    [SerializeField] private GameObject _waitLevelContent;
    [SerializeField] private GameObject _infoEnemyContent;
    private int _enemyAll;
    private int _enemiesLeft;

    [Header("Selected Panel")]
    [SerializeField] private GameObject _panelActiveObject;
    [SerializeField] private Transform _contentMainBase;
    [SerializeField] private Transform _contentTower;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _destroyButton;
    //MainBase Content
    [SerializeField] private TMP_Text _levelBase;
    [SerializeField] private TMP_Text _healthMainBase;
    //Tower Content
    [SerializeField] private TMP_Text _nameTower;
    [SerializeField] private TMP_Text _levelTower;   
    [SerializeField] private TMP_Text _damageTower;
    [SerializeField] private TMP_Text _radiusTower;
    [SerializeField] private TMP_Text _rateOfFire;
    [SerializeField] private TMP_Text _priceUpgradeTower;
    [Header("UI End Game")]
    [SerializeField] private float _timeInvokePanel;
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private Canvas _gameWinCanvas;
    private void Awake()
    {       
        EventAggregator.Subscribe<SelectedTowerEvent>(SelectedTower);
        EventAggregator.Subscribe<SelectedMainBaseEvent>(SelectedMainBase);
        EventAggregator.Subscribe<LevelMaxEvent>(LevelMaxChange);
        EventAggregator.Subscribe<SelectedBuildPointEvent>(SelectedBuildPointChange);
        EventAggregator.Subscribe<StartGameEvent>(StartGameUI);
        EventAggregator.Subscribe<MoneyUpdateUIEvent>(UpdateMoneyUIChange);
        EventAggregator.Subscribe<DeselectedAllEvent>(DeselectedAllChange);
        EventAggregator.Subscribe<UpdateInfoMainBaseEvent>(UpdateInfoMainBaseChange);
        EventAggregator.Subscribe<StartWaveEvent>(StartWaveChange);
        EventAggregator.Subscribe<UpdateInfoWaveEvent>(UpdateInfoWaveChange);
        EventAggregator.Subscribe<WaitWaweEvent>(WaitWaweChange);
        EventAggregator.Subscribe<NotEnoughMoneyEvent>(NotEnoughMoneyChange);
        EventAggregator.Subscribe<GameOverEvent>(GameOverChange);
        EventAggregator.Subscribe<GameWinEvent>(GameWinChange);
    }
    void Start()
    {
        _startLevelButton.onClick.AddListener(StartWaveButton);
        _destroyButton.onClick.AddListener(DestroyButton);
    }
    private void LevelMaxChange(object sender, LevelMaxEvent eventData)
    {
        _upgradeButton.interactable = false;
        if(eventData.IsUpgreded)
        _levelMaxText.GetComponent<Animator>().Play("Hide");
    }
    private void NotEnoughMoneyChange(object sender, NotEnoughMoneyEvent eventData)
    {
        _notMoneyText.GetComponent<Animator>().Play("Hide");
    }
    private void SelectedTower(object sender, SelectedTowerEvent eventData)
    {
        UpdateButtons();
        _contentMainBase.gameObject.SetActive(false);
        _contentTower.gameObject.SetActive(true);
        
        _destroyButton.gameObject.SetActive(true);

        _nameTower.text = eventData.Name;
        _levelTower.text = eventData.Level;
        _damageTower.text = string.Format("{0} - {1}", eventData.MinDamage, eventData.MaxDamage);
        _radiusTower.text = eventData.Radius;
        _rateOfFire.text = eventData.RateOfFire;
        _priceUpgradeTower.text = eventData.PriceUpgrade;
    }
    private void SelectedMainBase(object sender, SelectedMainBaseEvent eventData)
    {
        _contentMainBase.gameObject.SetActive(true);
        _contentTower.gameObject.SetActive(false);

        UpdateButtons();
        _destroyButton.gameObject.SetActive(false);
        _levelBase.text = eventData.Level;
        _healthMainBase.text = eventData.MaxHealthBase;
    }
    private void DestroyButton()
    {
        var tower = SelectedObjectController.CurrentSelectedObject.GetComponent<Tower>();
        var parent = tower.transform.parent;
        parent.GetChild(0).gameObject.SetActive(true);
        parent.GetComponent<BoxCollider>().enabled = true;

        EventAggregator.Post(this, new MoneyUpdateEvent() { MoneyCount = tower.TowerScriptable.PriceTower / 2 });
        EventAggregator.Post(this, new DeselectedAllEvent());
        Destroy(tower.gameObject);
    }
    private void UpdateButtons()
    {
        _upgradeButton.interactable = true;
        _upgradeButton.onClick.RemoveAllListeners();
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
    private void UpdateMoneyUIChange(object sender, MoneyUpdateUIEvent eventData)
    {
        _moneyText.text = eventData.Count.ToString();
    }
    private void SelectedBuildPointChange(object sender, SelectedBuildPointEvent eventData)
    {
        _panelActiveObject.SetActive(false);
        _panelTowersBuild.SetActive(true);
    }
    private void DeselectedAllChange(object sender, DeselectedAllEvent eventData)
    {
        _panelActiveObject.SetActive(false);
        _panelTowersBuild.SetActive(false);
    }
    private void UpdateInfoMainBaseChange(object sender, UpdateInfoMainBaseEvent eventData)
    {
        _healthSlider.maxValue = eventData.MaxHealthBase;        
        _healthSlider.value = eventData.CurrentHealth;

        int health = (int)eventData.CurrentHealth;
        _healthSliderText.text = health + " / " + eventData.MaxHealthBase;
    }
    private void StartWaveChange(object sender, StartWaveEvent eventData)
    {
        Time.timeScale = 1f;
        _waitLevelContent.SetActive(false);
        _infoEnemyContent.SetActive(true);
        _enemiesLeft = eventData.CountEnemyInWaveStart;
        _enemyAll = eventData.CountEnemyInWaveStart;

        _levelCurrentText.text = string.Format("Wave: {0}/{1}", eventData.CurrentWaveCount, eventData.AllWave);
        _enemiesCountText.text = string.Format("{0}/{1}", _enemiesLeft, _enemyAll);
    }
    private void UpdateInfoWaveChange(object sender, UpdateInfoWaveEvent eventData)
    {
        _enemiesLeft = eventData.EnemiesLeft;
        _enemiesCountText.text = string.Format("{0}/{1}", _enemiesLeft, _enemyAll);
    }
    private void WaitWaweChange(object sender, WaitWaweEvent eventData)
    {
        _waitLevelContent.SetActive(true);
        _infoEnemyContent.SetActive(false);
        _durationToNextLevel.maxValue = eventData.MaxTime;
        _durationToNextLevel.value = eventData.Timer;
    }
    private void StartWaveButton()
    {
        Time.timeScale = _timeScale;
    }
    private void StartGameUI(object sender, StartGameEvent eventData)
    {
        _canvasHUD.gameObject.SetActive(true);
        _gameOverCanvas.gameObject.SetActive(false);
        _gameWinCanvas.gameObject.SetActive(false);

        _panelActiveObject.SetActive(false);
        _panelTowersBuild.SetActive(false);

        _moneyText.text = eventData.StartMoney.ToString();

        _healthSlider.maxValue = MainBase.CurrentLevel.MaxHealth;
        _healthSlider.value = MainBase.CurrentHealthBase;

        _healthSliderText.text = string.Format("{0} / {1}", MainBase.CurrentHealthBase, MainBase.CurrentLevel.MaxHealth);
    }
    private void GameOverChange(object sender, GameOverEvent eventData)
    {
        _canvasHUD.gameObject.SetActive(false);
        Invoke(nameof(InvokeCanvasGameOver), _timeInvokePanel);
    }
    private void InvokeCanvasGameOver()
    {
        _gameOverCanvas.gameObject.SetActive(true);
    }
    private void GameWinChange(object sender, GameWinEvent eventData)
    {
        _canvasHUD.gameObject.SetActive(false);
        Invoke(nameof(InvokeCanvasGameWin), _timeInvokePanel);
    }
    private void InvokeCanvasGameWin()
    {       
        _gameWinCanvas.gameObject.SetActive(true);
    }
}
