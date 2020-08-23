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
    private GameObject buttonLoad;
    private GameObject buttonEditGround;
    private GameObject inventoryUI;

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

        if (groundEditable == null)
            groundEditable = GameObject.FindWithTag("GroundEditable");

        if (carController == null)
            carController = GameObject.FindWithTag("CarController");

        if (backgroundSprite == null)
            backgroundSprite = GameObject.FindWithTag("BackgroundSprite");

        if (buttonBrake == null)
            buttonBrake = GameObject.FindWithTag("ButtonBrake");

        if (buttonGas == null)
            buttonGas = GameObject.FindWithTag("ButtonGas");

        if (buttonReload == null)
            buttonReload = GameObject.FindWithTag("ButtonReload");

        if (buttonPanZoom == null)
            buttonPanZoom = GameObject.FindWithTag("ButtonPanZoom");

        if (buttonDrag == null)
            buttonDrag = GameObject.FindWithTag("ButtonDrag");

        if (buttonSave == null)
            buttonSave = GameObject.FindWithTag("ButtonSave");

        if (buttonLoad == null)
            buttonLoad = GameObject.FindWithTag("ButtonLoad");

        if (textMoney == null)
            textMoney = GameObject.FindWithTag("TextMoney");

        if (textDistance == null)
            textDistance = GameObject.FindWithTag("TextDistance");

        if (imageFuel == null)
            imageFuel = GameObject.FindWithTag("ImageFuel");

        if (buttonEditGround == null)
            buttonEditGround = GameObject.FindWithTag("ButtonEditGround");

        if (inventoryUI == null)
            inventoryUI = GameObject.FindWithTag("InventoryUI");


    }

    private void Start()
    {
        cameraStartupOrthographicSize = Camera.main.orthographicSize;
        setPlayMode();
    }

    public void OnPrefabDropEvent(GameObject prefab)
    {
        Debug.Log("Game logics, on prefab drop event");
        //Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity);

    }

    public void onReloadButtonReleased(bool pressed)
    {
        if (!pressed) // if released
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onPanZoomButton(bool pressed)
    {
        if (pressed)
        {
            Camera.main.GetComponent<PanZoom>().enabled = true;
            groundEditable.GetComponent<EditableGround>().enableEditing(false);
        }
        else
        {
            Camera.main.GetComponent<PanZoom>().enabled = false;
            bool edit = buttonEditGround.GetComponent<ButtonBistable>().GetState();
            groundEditable.GetComponent<EditableGround>().enableEditing(edit);
        }
    }

    public void onEditPlayButton(bool edit)
    {
        Debug.Log("Edit " + edit.ToString());
        if (edit)
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

    public void onEditGroundButton(bool editGround)
    {
        if (editGround)
        {
            groundEditable.GetComponent<EditableGround>().enableEditing(true);
        }
        else
        {
            groundEditable.GetComponent<EditableGround>().enableEditing(false);
        }
    }

    public void SaveLevel()
    {

        SaveObject saveObject = new SaveObject();

        //SAVE GROUND
        saveObject.splinePoints = groundEditable.GetComponent<EditableGround>().getSplinesPointsPositions();

        //SAVE COINS
        GameObject[] coins;
        coins = GameObject.FindGameObjectsWithTag("Coin");
        saveObject.coinsPositions = new List<Vector3>();

        foreach (var coin in coins)
        {
            saveObject.coinsPositions.Add(coin.transform.position);
        }

        //SAVE CANISTERS
        GameObject[] canisters;
        canisters = GameObject.FindGameObjectsWithTag("FuelCanister");
        saveObject.canisterPositions = new List<Vector3>();

        foreach (var canister in canisters)
        {
            saveObject.canisterPositions.Add(canister.transform.position);
        }

        SaveSystem.Save<SaveObject>(saveObject, "0.txt");
    }

    public void LoadLevel()
    {
        SaveObject saveObject = SaveSystem.Load<SaveObject>("0.txt");

        //LOAD GROUND
        if (saveObject == null)
            Debug.Log("Load retun null");
        else
            groundEditable.GetComponent<EditableGround>().loadPoints(saveObject.splinePoints);

        //LOAD COINS
        GameObject[] coins;
        coins = GameObject.FindGameObjectsWithTag("Coin");

        Vector3[] coinsPos = saveObject.coinsPositions.ToArray();

        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.position = coinsPos[i];
        }

        //LOAD CANISTERS
        GameObject[] canisters;
        canisters = GameObject.FindGameObjectsWithTag("FuelCanister");

        Vector3[] canisterPos = saveObject.canisterPositions.ToArray();

        for (int i = 0; i < canisters.Length; i++)
        {
            canisters[i].transform.position = canisterPos[i];
        }

    }



    public void onDragButton(bool drag)
    {
        if (mode == Mode.EDIT)
        {


        }
    }

    void setPlayMode()
    {

        backgroundSprite.SetActive(true);
        Camera.main.GetComponent<BackgroundStatic>().enabled = true;
        Camera.main.GetComponent<PanZoom>().enabled = false;
        Camera.main.orthographicSize = cameraStartupOrthographicSize;
        if (secondRun == false)
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
        buttonLoad.SetActive(false);
        buttonEditGround.SetActive(false);
        textMoney.SetActive(true);
        textDistance.SetActive(true);
        imageFuel.SetActive(true);
        inventoryUI.SetActive(false);

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
        buttonLoad.SetActive(true);
        buttonEditGround.SetActive(true);
        textMoney.SetActive(false);
        textDistance.SetActive(false);
        imageFuel.SetActive(false);
        inventoryUI.SetActive(true);

        groundEditable.SetActive(true);
        groundEditable.GetComponent<EditableGround>().enabled = true;

        bool edit = buttonEditGround.GetComponent<ButtonBistable>().GetState();
        groundEditable.GetComponent<EditableGround>().enableEditing(edit);

    }

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    private class SaveObject
    {
        public List<Vector3> splinePoints;
        public List<Vector3> coinsPositions;
        public List<Vector3> canisterPositions;
    }
}
