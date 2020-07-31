﻿    using UnityEngine;
    using System.Collections;
    using UnityEngine.EventSystems;
     
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     
    public bool buttonPressed;
     
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
            buttonPressed = false;
    }
    }

