using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIButton : Button
{
    [SerializeField] protected ButtonConfig buttonConfig;
    
    [HideInInspector] public UnityEvent onHold = new UnityEvent();
    
    

    public new void OnPointerDown(PointerEventData eventData)
    {
        onHold.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
//        onClickedUp.Invoke();
    }

}
