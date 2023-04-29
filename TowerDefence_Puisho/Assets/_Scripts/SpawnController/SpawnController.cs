using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnController
{
    private static Transform _parentForSpawnObject;
    private static Dictionary<string, LinkedList<GameObject>> _spawnObject = new();

    public static void PutObject(GameObject gameObject)
    {
        gameObject.SetActive(false);

        if (_spawnObject.ContainsKey(gameObject.name))
        {
            _spawnObject[gameObject.name].AddLast(gameObject);
        }
        else
        {
            _spawnObject[gameObject.name] = new LinkedList<GameObject>();
            _spawnObject[gameObject.name].AddLast(gameObject);
        }
        if (gameObject.transform.parent != _parentForSpawnObject)
            gameObject.transform.SetParent(_parentForSpawnObject);
    }

    public static GameObject GetObject(GameObject gameObject)
    {
        if (_spawnObject.ContainsKey(gameObject.name))
        {
            if (_spawnObject[gameObject.name].Count > 0)
            {
                var result = _spawnObject[gameObject.name].Last;
                _spawnObject[gameObject.name].RemoveLast();
                return result.Value;
            }
            else
            {
                return InstantiateObject(gameObject);
            }
        }
        else
        {
            _spawnObject[gameObject.name] = new LinkedList<GameObject>();
            return InstantiateObject(gameObject);
        }
    }

    public static void InitializePool(GameObject gameObjectPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var gameObject = InstantiateObject(gameObjectPrefab);
            PutObject(gameObject);
        }
    }

    public static void SetParentForObject(Transform parent)
    {
        _parentForSpawnObject = parent;
    }

    private static GameObject InstantiateObject(GameObject gameObject)
    {
        var result = GameObject.Instantiate(gameObject, _parentForSpawnObject);
        result.name = gameObject.name;
        return result;
    }
    public static void ClearAllOnScene()
    {
        _spawnObject.Clear();
    }
}
