using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 using UnityEngine.UI;

public class ButtonBistable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     
    public bool buttonPressed;

    public Sprite pressedSprite;
    public Sprite releasedSprite;
    Image myImageComponent;
    void Start () 
    {
        myImageComponent = GetComponent<Image>();
        myImageComponent.sprite = releasedSprite;
    }
 

    public void OnPointerDown(PointerEventData eventData)
    {

    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
    
        buttonPressed = !buttonPressed;
        if(buttonPressed)
            myImageComponent.sprite =  pressedSprite;
        else
            myImageComponent.sprite = releasedSprite;
    }
}
