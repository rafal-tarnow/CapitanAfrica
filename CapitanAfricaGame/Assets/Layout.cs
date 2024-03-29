﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Layout :  MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform rightButton;
    public RectTransform centerButton;
    public RectTransform leftButton;
    public RectTransform titleTextRt;
    private RectTransform transform;

    public Vector2 buttonLeftPos = new Vector2(0.2f, 0.2f);
    public Vector2 buttonCenterPos = new Vector2(0.5f, 0.2f);
    public Vector2 buttonRightPos = new Vector2(0.8f, 0.2f);    
    float width;
    float height;

    void Start()
    {
        if(transform == null)
            transform = GetComponent<RectTransform>();

        Update();
    }

    float calcYPos(float procent)
    {
        return height*procent - height/2.0f;
    }

    float calcXPos(float procent)
    {
        return width*procent - width/2.0f;
    }

    float calcWidth(float procent)
    {
        return width*procent;
    }

    float calcHeight(float procent)
    {
        return height*procent;
    }

    #warning Read how to update sizez only on rezise changed
    void Update()
    {
        width = transform.rect.width;
        height = transform.rect.height;

        if(leftButton != null)
        {
            leftButton.localPosition = new Vector3(calcXPos(buttonLeftPos.x),calcYPos(buttonLeftPos.y),0);
            leftButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));
        }

        if(centerButton != null)
        {
            centerButton.localPosition = new Vector3(calcXPos(buttonCenterPos.x),calcYPos(buttonCenterPos.y),0);
            centerButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));
        }

        if(rightButton != null)
        {
            rightButton.localPosition = new Vector3(calcXPos(buttonRightPos.x),calcYPos(buttonRightPos.y),0);
            rightButton.sizeDelta = new Vector2 (calcWidth(0.2f), calcHeight(0.2f));
        }

        if(titleTextRt != null)
        {
            titleTextRt.localPosition = new Vector3(calcXPos(0.5f),calcYPos(0.9f), 0);
            titleTextRt.sizeDelta = new Vector2 (calcWidth(0.9f), calcHeight(0.1f));
        }
    }
}
