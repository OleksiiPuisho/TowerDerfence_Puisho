using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    void Update()
    {
        gameObject.transform.LookAt(_target);
    }
}
