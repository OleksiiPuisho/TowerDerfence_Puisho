using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class StartLevel : MonoBehaviour
{
    [SerializeField] private List<InitializeData> _initializeDatas = new();
    [SerializeField] private Transform _globalParent;
    [SerializeField] private int _startMoney;
    [SerializeField] private bool _isTutorial;
    [SerializeField] private Canvas _canvasTuturial;
    public void CloseTuturialCanvas()
    {
        _canvasTuturial.enabled = false;
        Time.timeScale = 1f;
        EventAggregator.Post(this, new StartGameEvent() { StartMoney = _startMoney });
    }
    void Awake()
    {
        SpawnController.ClearAllOnScene();
        InitializeSpawn();
    }
    private void Start()
    {
        if (!_isTutorial)
        {
            _canvasTuturial.enabled = false;
            EventAggregator.Post(this, new StartGameEvent() { StartMoney = _startMoney });
        }
        else
        {
            Time.timeScale = 0f;
            _canvasTuturial.enabled = true;
        }
    }
    private void InitializeSpawn()
    {
        SpawnController.SetParentForObject(_globalParent);

        foreach(var data in _initializeDatas)
        {
            SpawnController.InitializePool(data.Prefab, data.Count);
        }
    }
}
[System.Serializable]
public class InitializeData
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _count;

    public GameObject Prefab => _prefab;
    public int Count => _count;
}
