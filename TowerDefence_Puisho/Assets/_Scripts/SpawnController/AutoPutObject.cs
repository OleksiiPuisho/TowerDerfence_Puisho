using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPutObject : MonoBehaviour
{
    [SerializeField] private int _autoPutTime;
    private void AutoPut()
    {
        Invoke(nameof(AutoPutHandler), _autoPutTime);
    }
    private void AutoPutHandler()
    {
        SpawnController.PutObject(gameObject);
    }
    private void OnEnable()
    {
        AutoPut();
    }
}
