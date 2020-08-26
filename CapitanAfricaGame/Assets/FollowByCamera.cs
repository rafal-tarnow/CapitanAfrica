using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowByCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        this.transform.position = new Vector3(camPos.x, camPos.y, 0f);
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
