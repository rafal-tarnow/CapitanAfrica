using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceText : MonoBehaviour
{
    // Start is called before the first frame update
    private static Text text;
    private static int currentDistance = 0;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = "Dist " + currentDistance.ToString() + "m";
    }

    public static void setDistance(int distance)
    {
        if(currentDistance != distance) // optymalizations, dont update text if dont changed
        {
            currentDistance = distance;
            text.text = "Dist " + currentDistance.ToString() + "m";
        }
    }
}
