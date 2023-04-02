using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectedObject : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,IPointerUpHandler
{
    public GameObject SelectedIcon;
    public void OnPointerClick(PointerEventData eventData)
    {
        SelectedObjectController.CurrentSelectedObject = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
    void Update()
    {
        if(SelectedObjectController.CurrentSelectedObject != this && SelectedIcon.activeSelf == true)
        {
            SelectedIcon.SetActive(false);
        }
    }
}
public enum TypeObject { Tower, MainBase}
