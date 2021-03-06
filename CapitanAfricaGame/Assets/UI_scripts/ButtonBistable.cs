﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;


public class ButtonBistable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public UnityEvent<bool> OnBistableButtonEvent;



    private bool buttonPressed = false;

    public Sprite pressedSprite;
    public Sprite releasedSprite;
    Image myImageComponent;


    public void Awake()
    {
        myImageComponent = GetComponent<Image>();
        myImageComponent.sprite = releasedSprite;

        if (OnBistableButtonEvent == null)
            OnBistableButtonEvent = new UnityEvent<bool>();
    }

    void Start()
    {

    }

    public void SetStateWithoutEvent(bool state)
    {
        buttonPressed = state;

        if (buttonPressed)
            myImageComponent.sprite = pressedSprite;
        else
            myImageComponent.sprite = releasedSprite;
    }

    public bool GetState()
    {
        return buttonPressed;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = !buttonPressed;

        OnBistableButtonEvent?.Invoke(buttonPressed);

        if (buttonPressed)
            myImageComponent.sprite = pressedSprite;
        else
            myImageComponent.sprite = releasedSprite;

    }
}
