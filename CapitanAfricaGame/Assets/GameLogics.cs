using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
   private GameObject buttonReload;
   private GameObject buttonPanZoom;

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

            if(buttonReload == null)
                buttonReload = GameObject.FindWithTag("ButtonReload");

            if(buttonPanZoom == null)
                buttonPanZoom = GameObject.FindWithTag("ButtonPanZoom");

        
        }

        private void Start() 
        {
            setPlayMode();
        }

    public void onReloadButtonReleased(bool pressed)
    {
        if(!pressed) // if released
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void onPanZoomButton(bool pressed)
    {
        if(pressed)
        {
            Camera.main.GetComponent<PanZoom>().enabled = true;
            groundEditable.GetComponent<EditableGround>().enabled = false;
        }
        else
        {
            Camera.main.GetComponent<PanZoom>().enabled = false;
            groundEditable.GetComponent<EditableGround>().enabled = true;
        }
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
            buttonReload.SetActive(true);
            buttonPanZoom.SetActive(false);
    }

    void setEditMode()
    {
            Camera.main.GetComponent<BackgroundStatic>().enabled = false;
            Camera.main.GetComponent<PanZoom>().enabled = false;
            groundEditable.SetActive(true);
            ground.SetActive(false);
            carController.SetActive(false);
            backgroundSprite.SetActive(false);
            buttonGas.SetActive(false);
            buttonBrake.SetActive(false);
            buttonReload.SetActive(false);
            buttonPanZoom.SetActive(true);
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
