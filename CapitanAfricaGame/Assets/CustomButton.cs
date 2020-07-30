    using UnityEngine;
    using System.Collections;
    using UnityEngine.EventSystems;
     
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     
    public bool buttonPressed;
     
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Gas is pressed");
        buttonPressed = true;
    }
     
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Gas is released");
            buttonPressed = false;
        }
    }

