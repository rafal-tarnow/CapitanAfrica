using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameLogics : MonoBehaviour
{

    enum Mode 
    {
        PLAY,
        EDIT,
    }
    Mode mode = Mode.PLAY;

    private static string SAVE_FOLDER;

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
   private GameObject buttonDrag;

   private GameObject buttonSave;
   private GameObject textMoney;
   private GameObject textDistance;
   private GameObject imageFuel;

   private bool secondRun = false;
    float cameraStartupOrthographicSize;
  
        void Awake() 
        {
            SaveSystem.Init();

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

            if(buttonDrag == null)
                buttonDrag = GameObject.FindWithTag("ButtonDrag");

            if(buttonSave == null)
                buttonSave = GameObject.FindWithTag("ButtonSave");

            if(textMoney == null)
                textMoney = GameObject.FindWithTag("TextMoney");

            if(textDistance == null)
                textDistance = GameObject.FindWithTag("TextDistance");

            if(imageFuel == null)
                imageFuel = GameObject.FindWithTag("ImageFuel");

        }

    private void Start() 
    {
        cameraStartupOrthographicSize = Camera.main.orthographicSize;
        setPlayMode();
    }

    public void onReloadButtonReleased(bool pressed)
    {
        if(!pressed) // if released
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }


    public void SaveLevel()
    {

            SaveObject saveObject = new SaveObject();
            saveObject.splinePoints = groundEditable.GetComponent<EditableGround>().getSplinesPointsPositions();

            SaveSystem.Save<SaveObject>(saveObject);        
    }

    public void LoadLevel()
    {
        SaveObject saveObject = SaveSystem.Load<SaveObject>();
        if(saveObject == null)
            Debug.Log("Load retun null");
        else
            groundEditable.GetComponent<EditableGround>().loadPoints(saveObject.splinePoints);
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
            mode = Mode.EDIT;
            setEditMode();
        }
        else
        {
            mode = Mode.PLAY;
            setPlayMode();
            carController.transform.position = Camera.main.transform.position;
        }
    }

    public void onDragButton(bool drag)
    {
        if(mode == Mode.EDIT)
        {
            if(drag)
                groundEditable.GetComponent<EditableGround>().blockAddNewPoints(true);
            else
                groundEditable.GetComponent<EditableGround>().blockAddNewPoints(false);

        }
    }

    void setPlayMode()
    {

            backgroundSprite.SetActive(true);
            Camera.main.GetComponent<BackgroundStatic>().enabled = true;
            Camera.main.GetComponent<PanZoom>().enabled = false;
            Camera.main.orthographicSize = cameraStartupOrthographicSize;
         if(secondRun == false)
        {           
            groundEditable.SetActive(false);
            ground.SetActive(true);
            secondRun = true;
        }
        else
        {
            groundEditable.SetActive(true);
            groundEditable.GetComponent<EditableGround>().enabled = false;
            ground.SetActive(false);
        }
            carController.SetActive(true);
            buttonGas.SetActive(true);
            buttonBrake.SetActive(true);
            buttonReload.SetActive(true);
            buttonPanZoom.SetActive(false);
            buttonDrag.SetActive(false);
            buttonSave.SetActive(false);
            textMoney.SetActive(true);
            textDistance.SetActive(true);
            imageFuel.SetActive(true);

    }

    void setEditMode()
    {
            Camera.main.GetComponent<BackgroundStatic>().enabled = false;
            Camera.main.GetComponent<PanZoom>().enabled = false;

            ground.SetActive(false);
            carController.SetActive(false);
            backgroundSprite.SetActive(false);
            buttonGas.SetActive(false);
            buttonBrake.SetActive(false);
            buttonReload.SetActive(false);
            buttonPanZoom.SetActive(true);
            buttonDrag.SetActive(true);
            buttonSave.SetActive(true);
            textMoney.SetActive(false);
            textDistance.SetActive(false);
            imageFuel.SetActive(false);

            groundEditable.SetActive(true);
            groundEditable.GetComponent<EditableGround>().enabled = true;
            LoadLevel();
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    private class SaveObject{
        public List<Vector3> splinePoints;
    }
}
