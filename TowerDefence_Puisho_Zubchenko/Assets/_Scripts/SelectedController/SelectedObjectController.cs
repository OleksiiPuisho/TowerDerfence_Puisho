using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectedObjectController : MonoBehaviour
{
    public static SelectedObject CurrentSelectedObject;
    public static event Action IsSelected = delegate { };
    public void DeselectedAll()
    {
        if(CurrentSelectedObject != null)
        CurrentSelectedObject = null;
    }
    void Awake()
    {
        
    }

    void Update()
    {
        if(CurrentSelectedObject != null && CurrentSelectedObject.SelectedIcon.activeSelf == false)
        {
            IsSelected.Invoke();
            CurrentSelectedObject.SelectedIcon.SetActive(true);
        }
    }
}
