using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private Material _material;
    [SerializeField] private float _speedRate;
    public bool Dissolve;
    void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        if(Dissolve)
            _material.SetFloat("_Ammount", 0f);
        else
            _material.SetFloat("_Ammount", 1f);
    }
    void Update()
    {
        if (Dissolve)
        {
            _material.SetFloat("_Ammount", _material.GetFloat("_Ammount") + _speedRate * Time.deltaTime);
            if (_material.GetFloat("_Ammount") > 0.9f)
            {
                _material.SetFloat("_Ammount", 1f);
                enabled = false;
            }
        }
        else
        {
            _material.SetFloat("_Ammount", _material.GetFloat("_Ammount") - _speedRate * Time.deltaTime);
            if (_material.GetFloat("_Ammount") < 0.01f)
            {
                _material.SetFloat("_Ammount", 0f);
                enabled = false;
            }
        }
    }
}
