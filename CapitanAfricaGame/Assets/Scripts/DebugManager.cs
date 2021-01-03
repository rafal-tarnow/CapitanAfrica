using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


class DebugManager
{
    private bool debugEnable = true;
    private TMP_Text text_0;
    private TMP_Text text_1;
    private TMP_Text text_2;
    private TMP_Text text_3;

    public void Start() 
    {
        if(!debugEnable)
            return;

        if(text_0 == null)
            text_0 = GameObject.FindWithTag("DbgTxt_1").GetComponent<TMP_Text>();

        if(text_1 == null)
            text_1 = GameObject.FindWithTag("DbgTxt_2").GetComponent<TMP_Text>();

        if(text_2 == null)
            text_2 = GameObject.FindWithTag("DbgTxt_3").GetComponent<TMP_Text>();

        if(text_3 == null)
            text_3 = GameObject.FindWithTag("DbgTxt_4").GetComponent<TMP_Text>();


    }

        float fixedTime = 0;
        public void FixedUpdate() 
        {
             if(!debugEnable)
                 return;
            
            float deltaTime = Time.time - fixedTime;
            fixedTime = Time.time;
            text_0.SetText("FUDeltaTime  = {0}", deltaTime);
        }

        float updateTime = 0;
        public void Update()
         {
             if(!debugEnable)
                 return;

            float deltaTime = Time.time - updateTime;
            updateTime = Time.time;
            text_1.SetText("UDeltaTime = {0}", deltaTime);
            text_2.SetText("");
            updateTime = Time.time;
        }

        float lateUpdate = 0;
        public void LateUpdate()
        {
            float deltaTime = Time.time - fixedTime;
            lateUpdate = Time.time;
            text_3.SetText("FU to LU = {0}", deltaTime);
        }
    }
