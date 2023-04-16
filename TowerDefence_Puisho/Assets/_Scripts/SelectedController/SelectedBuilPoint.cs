using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Helpers;
using Helpers.Events;

public class SelectedBuilPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Transform SpawnTower;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(BuildPointsController.ActivePoint != null)
        {
            BuildPointsController.ActivePoint.GetComponent<Animator>().SetBool("IsSelected", false);
        }
        BuildPointsController.ActivePoint = this;
        EventAggregator.Post(this, new SelectedBuildPointEvent());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
