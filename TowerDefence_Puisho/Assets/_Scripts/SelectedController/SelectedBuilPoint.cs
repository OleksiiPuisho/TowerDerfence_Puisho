using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SelectedBuilPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Transform SpawnTower;
    public void OnPointerClick(PointerEventData eventData)
    {
        BuildPointsController.ActivePoint = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
