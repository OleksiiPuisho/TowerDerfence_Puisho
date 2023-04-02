using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHUD : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TMP_Text _healthSliderText;
    [SerializeField] private TMP_Text _moneyText;

    [Header("Selected Panel")]
    [SerializeField] private Transform _contentMainBase;
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
    }
    void ObjectSelected()
    {
        
    }
}
