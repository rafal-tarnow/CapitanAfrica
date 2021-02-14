using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(0.35f + 0.05f* Mathf.Sin(5.0f* Time.timeSinceLevelLoad), 0.35f + 0.05f * Mathf.Sin(5.0f * Time.timeSinceLevelLoad), 1.0f);
    }
}
