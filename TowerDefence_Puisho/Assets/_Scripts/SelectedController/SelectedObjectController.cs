using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Helpers;
using Helpers.Events;

public class SelectedObjectController : MonoBehaviour
{
    public static SelectedObject CurrentSelectedObject;
    public void DeselectedAll()
    {
        if (CurrentSelectedObject != null)
        {
            CurrentSelectedObject.SelectedIcon.SetActive(false);
            CurrentSelectedObject = null;
        }
        EventAggregator.Post(this, new DeselectedAllEvent());
    }
    void Awake()
    {
        EventAggregator.Subscribe<SelectedMainBaseEvent>(SelectedMainBaseChange);
        EventAggregator.Subscribe<SelectedTowerEvent>(SelectedTowerChange);
        EventAggregator.Subscribe<DeselectedAllEvent>(DeselectedAllChange);
    }
    private void SelectedMainBaseChange(object sender, SelectedMainBaseEvent eventData)
    {
        CurrentSelectedObject.SelectedIcon.SetActive(true);
    }
    private void SelectedTowerChange(object sender, SelectedTowerEvent eventData)
    {
        CurrentSelectedObject.SelectedIcon.SetActive(true);
    }
    private void DeselectedAllChange(object sender, DeselectedAllEvent eventData)
    {
        if (CurrentSelectedObject != null)
        {
            CurrentSelectedObject.SelectedIcon.SetActive(false);
            CurrentSelectedObject = null;
        }
    }
}
