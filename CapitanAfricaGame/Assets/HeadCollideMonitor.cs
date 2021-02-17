using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadCollideMonitor : MonoBehaviour
{
    public Joint2D joint;
    public UnityEvent jointBreakEvent;


    private void Start() {
        if(jointBreakEvent == null)
            jointBreakEvent = new UnityEvent();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
    //     Debug.Log("OnCollisionEnter2D");
    Layer layer = col.gameObject.GetComponent<Layer>();
    if(layer != null)
    {
        if(layer.checkLayer(Layer.Type.HEAD_BRAKE))
        {
            if(joint != null)
            {
                Destroy(joint);
                joint = null;
                jointBreakEvent.Invoke();
            }
        }
    }
    }
}
