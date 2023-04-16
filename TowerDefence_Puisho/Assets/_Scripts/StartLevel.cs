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
    void Awake()
    {
        InitializeSpawn();
        EventAggregator.Post(this, new StartGameEvent() { StartMoney = _startMoney});
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
