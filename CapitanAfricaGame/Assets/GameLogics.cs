using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogics : MonoBehaviour
{
    public bool cameraFollowEnable = false;


   private static GameLogics gameLogics;
   private GameObject ground;
   private GameObject groundEditable;
   private GameObject carController;
   private GameObject backgroundSprite;
   private GameObject buttonGas;
   private GameObject buttonBrake;


        void Awake() 
        {
            if (ground == null)
                ground = GameObject.FindWithTag("Ground");

            if(groundEditable == null)
                groundEditable = GameObject.FindWithTag("GroundEditable");

            if(carController == null)
                carController = GameObject.FindWithTag("CarController");

            if(backgroundSprite == null)
                backgroundSprite = GameObject.FindWithTag("BackgroundSprite");    

            if(buttonBrake == null)
                buttonBrake = GameObject.FindWithTag("ButtonBrake");

            if(buttonGas == null)
                buttonGas = GameObject.FindWithTag("ButtonGas");
        
        }

        private void Start() 
        {
            setPlayMode();
        }

    public void onEditPlayButton(bool edit)
    {
        Debug.Log("Edit " + edit.ToString());
        if(edit)
        {
            setEditMode();
        }
        else
        {
            setPlayMode();
        }
    }

    void setPlayMode()
    {
            backgroundSprite.SetActive(true);
            Camera.main.GetComponent<BackgroundStatic>().enabled = true;
            Camera.main.GetComponent<PanZoom>().enabled = false;
            groundEditable.SetActive(false);
            ground.SetActive(true);
            carController.SetActive(true);
            buttonGas.SetActive(true);
            buttonBrake.SetActive(true);
    }

    void setEditMode()
    {
            Camera.main.GetComponent<BackgroundStatic>().enabled = false;
            Camera.main.GetComponent<PanZoom>().enabled = true;
            groundEditable.SetActive(true);
            ground.SetActive(false);
            carController.SetActive(false);
            backgroundSprite.SetActive(false);
            buttonGas.SetActive(false);
            buttonBrake.SetActive(false);
    }
    public void onButton()
    {
        Debug.Log("onButton");
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
