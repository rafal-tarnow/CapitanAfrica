using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchDeleteSprite : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {

    }

    private void OnMouseDown()
    {

    }

    private void OnMouseUp()
    {
        if(!this.enabled)
            return;
            
        Debug.Log("TouchDeleteSprite on mouse up");
        Destroy(gameObject);
    }

}
