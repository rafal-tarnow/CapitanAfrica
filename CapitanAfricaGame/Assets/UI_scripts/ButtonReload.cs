using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonReload : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
     

     
    public void OnPointerDown(PointerEventData eventData)
    {

    }
     
    public void OnPointerUp(PointerEventData eventData)
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
}
