using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanvasMetaLogics : MonoBehaviour
{

    public UnityEvent retryEvent;

    private void Start() {
        if(retryEvent == null)
         retryEvent = new UnityEvent();    
    }

    public void onRetryButtonPressed()
    {
        Debug.Log("On Rerty");
        retryEvent.Invoke();
    }

    public void onMenuButtonPressed()
    {
        Debug.Log("On Menu");
    }

    public void onNextLevelButtonPressed()
    {
        Debug.Log("On Next");
    }
}
