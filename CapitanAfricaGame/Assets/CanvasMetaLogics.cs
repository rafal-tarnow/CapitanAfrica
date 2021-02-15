using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CanvasMetaLogics : MonoBehaviour
{

    public TMP_Text title;
    public Layout layout;
    public enum Type
    {
        META_REACHED,
        CRASH
    }
    public UnityEvent retryEvent;
    public UnityEvent menuEvent;
    public UnityEvent nextLevelEvent;

    public GameObject buttonRetryGO;
    public GameObject buttonMenuGO;
    public GameObject buttonNextGO;

    private void Start() {
        if(retryEvent == null)
         retryEvent = new UnityEvent();    

        if(menuEvent == null)
            menuEvent = new UnityEvent();    

        if(nextLevelEvent == null)
            nextLevelEvent = new UnityEvent();    
    }

    public void setType(Type type)
    {
        if(type == Type.META_REACHED)
        {
            buttonNextGO.SetActive(true);
            
            layout.buttonLeftPos = new Vector2(0.2f, 0.2f);
            layout.buttonCenterPos = new Vector2(0.5f, 0.2f);

            title.text = "Level completed !";
            title.color = new Color32(0, 255, 0, 255); 
        }
        else if(type == Type.CRASH)
        {
            buttonNextGO.SetActive(false);

            layout.buttonLeftPos = new Vector2(0.333f, 0.2f);
            layout.buttonCenterPos = new Vector2(0.6667f, 0.2f);

            title.text = "CRASH !!!";
            title.color = new Color32(255, 0, 0, 255); 
        }
    }

    public void onRetryButtonPressed()
    {
        Debug.Log("On Rerty");
        retryEvent.Invoke();
    }

    public void onMenuButtonPressed()
    {
        Debug.Log("On Menu");
        menuEvent.Invoke();
    }

    public void onNextLevelButtonPressed()
    {
        Debug.Log("On Next");
        nextLevelEvent.Invoke();
    }
}
