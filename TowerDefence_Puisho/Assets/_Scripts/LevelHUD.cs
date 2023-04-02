using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHUD : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthSliderText;
    [SerializeField] private TMP_Text _moneyText;

    [Header("Selected Panel")]
    [SerializeField] private Animator _panelActiveObject;
    [SerializeField] private Transform _contentMainBase;
    [SerializeField] private Transform _contentTower;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _levelMaxButton;
    [SerializeField] private Button _destroyButton;
    //MainBase Content
    [SerializeField] private TMP_Text _levelBase;
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _armor;
    void Start()
    {
        SelectedObjectController.IsSelected += ObjectSelected;
    }

    
    void Update()
    {
        _moneyText.text = GameController.Money.ToString();
        _healthSlider.maxValue = MainBase.MaxHealth;
        _healthSlider.value = MainBase.CurrentHealth;
        _healthSliderText.text = MainBase.CurrentHealth.ToString() + "/" + MainBase.MaxHealth.ToString();
        if(SelectedObjectController.CurrentSelectedObject == null && _panelActiveObject.gameObject.activeSelf == true)
        {
            _panelActiveObject.Play("Close");
            _panelActiveObject.gameObject.SetActive(false);
        }
        if(_panelActiveObject.gameObject.activeSelf == true)
        {
            if(SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.MainBase)
            {
                _destroyButton.gameObject.SetActive(false);
                _contentMainBase.gameObject.SetActive(true);
                _contentTower.gameObject.SetActive(false);

                _levelBase.text = MainBase.CurrentLevel.Level.ToString()[6..];
                _health.text = MainBase.CurrentHealth.ToString() + "/" + MainBase.MaxHealth.ToString();
                _armor.text = MainBase.Armor.ToString();
                if(MainBase.CurrentLevel.Level == Level.Level_2)
                {
                    _levelMaxButton.gameObject.SetActive(true);
                }
            }
            if (SelectedObjectController.CurrentSelectedObject.TypeObject == TypeObject.Tower)
            {
                _destroyButton.gameObject.SetActive(true);
            }
        }
    }
    void ObjectSelected()
    {
        _panelActiveObject.gameObject.SetActive(true);
    }
}
