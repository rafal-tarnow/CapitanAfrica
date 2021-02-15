using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JointMonitor : MonoBehaviour
{
    // Start is called before the first frame update    
    public UnityEvent jointBreakEvent;

    private void Start() {
        if(jointBreakEvent == null)
            jointBreakEvent = new UnityEvent();    
    }
    void OnJointBreak2D(Joint2D brokenJoint)
    {
        Debug.Log("JointMonitor on joint break");
        jointBreakEvent.Invoke();
    }
}
