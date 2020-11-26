using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceTextMP : MonoBehaviour
{
    private static TMP_Text text;
    private static int currentDistance = 0;

    private void Awake() 
    {
        if(text == null)
            text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        //text.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);
        text.SetText("Dist {0}m", 0);
    }

    public static void setDistance(int distance)
    {
        if(currentDistance != distance) // optymalizations, dont update text if dont changed
        {
            currentDistance = distance;
            text.SetText("Dist {0}m", currentDistance);
        }
    }
}
