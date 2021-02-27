using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public enum Type{
        HEAD_BRAKE = (1 << 0),
        PLAYER = (1 << 1)
    }

    public Type[] mLayer;
    private uint layerMask = 0b_0000_0000_0000_0000_0000_0000_0000_0000; //32bits
    private void Start() {
        for(int i = 0; i < mLayer.Length; i++)
        {
            layerMask |= (uint)(mLayer[i]);
        }
        Debug.Log("layerMask = " + layerMask.ToString());

    }

    public bool checkLayer(Type layer)
    {
        if((layerMask & ((uint)(layer))) != 0)
            return true;
        return false;
    }

}
