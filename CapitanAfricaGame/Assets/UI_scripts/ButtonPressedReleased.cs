using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonPressedReleased : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     
     public UnityEvent<bool> OnButtonPressedReleasedEvent;
     
    void Start () 
    {
        if (OnButtonPressedReleasedEvent == null)
            OnButtonPressedReleasedEvent = new UnityEvent<bool>();    
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressedReleasedEvent?.Invoke(true);
    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonPressedReleasedEvent?.Invoke(false);
    }
}
