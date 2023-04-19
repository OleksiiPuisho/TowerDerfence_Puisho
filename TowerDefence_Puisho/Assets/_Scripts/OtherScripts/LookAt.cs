using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [Header("Block Axses")]
    [SerializeField] private bool _x;
    [SerializeField] private bool _y;
    [SerializeField] private bool _z;
    private Vector3 _targetLook;
    private void Awake()
    {
        
    }
    void Update()
    {
        UpdateVector();
        gameObject.transform.LookAt(_targetLook);
    }
    private void UpdateVector()
    {
        if (!_x && !_y && !_z)
        {
            _targetLook = _target.position;
            return;
        }
        if (_x)
        {
            _targetLook = new(0, _target.position.y, _target.position.z);
            return;
        }
        if (_y)
        {
            _targetLook = new(_target.position.x, 0, _target.position.z);
            return;
        }
        if (_z)
        {
            _targetLook = new(_target.position.x, _target.position.y, 0);
            return;
        }
    }
}
