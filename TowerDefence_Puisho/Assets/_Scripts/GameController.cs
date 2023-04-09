using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int Money;
    [SerializeField] private int _startMoney;
    void Awake()
    {
        Money = _startMoney;
    }

    
    void Update()
    {
        
    }
}
