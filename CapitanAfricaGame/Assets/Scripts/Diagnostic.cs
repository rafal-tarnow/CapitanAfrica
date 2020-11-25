using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class Diagnostic : MonoBehaviour
{
    PerformanceCounter cpuCounter;
    TMP_Text cpuUsage;
    // Start is called before the first frame update
    void Start()
    {


         cpuUsage = GetComponent<TMP_Text>();

        //TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
        //textmeshPro.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);
        cpuCounter = new PerformanceCounter();
         cpuCounter.CategoryName = "Processor";
         cpuCounter.CounterName = "% Processor Time";
         cpuCounter.InstanceName = "_Total";
    }


    int value;
    // Update is called once per frame
    void Update()
    {
        if((value++)%10 == 0)
        cpuUsage.SetText(getCurrentCpuUsage());

    }

         public string getCurrentCpuUsage()
         {
                 return cpuCounter.NextValue()+"%";
     }
}
