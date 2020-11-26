    using UnityEngine;
    using System.Collections;
    using UnityEngine.EventSystems;
    using UnityEngine.Events;
    using UnityEngine.UIElements;
     
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     
    public UnityEvent<bool> OnButtonEvent;
    private Button buttonScript;
     
    private void Start() 
    {
        if (OnButtonEvent == null)
            OnButtonEvent = new UnityEvent<bool>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonEvent?.Invoke(true);
    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonEvent?.Invoke(false);
    }
}

