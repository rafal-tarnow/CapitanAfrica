using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointMonitor : MonoBehaviour
{
    // Start is called before the first frame update    
    void OnJointBreak2D(Joint2D brokenJoint)
    {
        Debug.Log("A joint has just been broken!");
        Debug.Log("The broken joint exerted a reaction force of " + brokenJoint.reactionForce);
        Debug.Log("The broken joint exerted a reaction torque of " + brokenJoint.reactionTorque);
    }
}
