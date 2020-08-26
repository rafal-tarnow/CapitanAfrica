using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameLogics : MonoBehaviour
{

    enum MainMode
    {
        PLAY,
        EDIT,
    }
    MainMode mode = MainMode.PLAY;

    enum EditSubMode
    {
        ADD_FANT,
        EDIT_GROUND,
        NONE
    }
    EditSubMode editSubMode = EditSubMode.NONE;

    private static string SAVE_FOLDER;

    public bool cameraFollowEnable = false;

    public GameObject coinPrefab;
    public GameObject canisterPrefab;

    private static GameLogics gameLogics;
    private GameObject groundEditable;
    private GameObject carController;
    private GameObject backgroundSprite;
    private GameObject buttonGas;
    private GameObject buttonBrake;
    private GameObject buttonReload;
    private GameObject buttonDrag;
    private GameObject buttonLoad;
    private GameObject buttonEditGround;
    private GameObject inventoryUI;

    private GameObject buttonSave;
    private GameObject textMoney;
    private GameObject textDistance;
    private GameObject imageFuel;
    float cameraStartupOrthographicSize;

    void Awake()
    {
        SaveSystem.Init();

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

        mode = MainMode.PLAY;
        editSubMode = EditSubMode.NONE;


        LoadLevel();
        updateUIState();
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

    public void OnUIInventoryDragEvent(bool isDragging)
    {
        Camera.main.GetComponent<PanZoom>().enabled = !isDragging; //if inventory is dragging disable pan zoom
    }
    public void OnDragRedDotEvent(bool isDragging)
    {
        Camera.main.GetComponent<PanZoom>().enabled = !isDragging; //if red dot is dragging disable pan zoom
    }

    public void OnDragSprite(bool isDragging)
    {
        Camera.main.GetComponent<PanZoom>().enabled = !isDragging;
    }


    public void onEditPlayButton(bool edit)
    {
        Debug.Log("Edit " + edit.ToString());
        if (edit)
        {
            mode = MainMode.EDIT;
            LoadLevel();
            updateUIState();
        }
        else
        {
            mode = MainMode.PLAY;
            SaveLevel();
            updateUIState();
            carController.transform.position = Camera.main.transform.position;
        }
    }

    public void onEditGroundButton(bool state)
    {
        if (state)
        {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.EDIT_GROUND))
            {
                editSubMode = EditSubMode.EDIT_GROUND;
                updateUIState();
            }
        }
        else
        {
            editSubMode = EditSubMode.NONE;
            updateUIState();
        }

    }

    public void onDragButton(bool state)
    {
        if (state)
        {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.ADD_FANT))
            {
                editSubMode = EditSubMode.ADD_FANT;
                updateUIState();
            }
        }
        else
        {
            editSubMode = EditSubMode.NONE;
            updateUIState();
        }
    }

    public void onPanZoomButton(bool state)
    {
        // if (state)
        // {
        //     if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.EDIT_PAN_ZOOM))
        //     {
        //         editSubMode = EditSubMode.EDIT_PAN_ZOOM;
        //         updateUIState();
        //     }
        // }
        // else 
        // {
        //     editSubMode = EditSubMode.EDIT_PAN_ZOOM;
        //     updateUIState();
        // }
    }

    private void updateUIState()
    {
        if (mode == MainMode.PLAY)
        {

            backgroundSprite.SetActive(true);
            Camera.main.GetComponent<BackgroundStatic>().enabled = true;
            Camera.main.GetComponent<PanZoom>().enabled = false;
            Camera.main.orthographicSize = cameraStartupOrthographicSize;

            groundEditable.SetActive(true);
            groundEditable.GetComponent<EditableGround>().enabled = false; // in play mode edition is disabled

            carController.SetActive(true);
            // carController.GetComponent<FollowByCamera>().enabled = false;
            // carController.GetComponent<CarController>().enabled = true;

            buttonGas.SetActive(true);
            buttonBrake.SetActive(true);
            buttonReload.SetActive(true);
            buttonDrag.SetActive(false);
            buttonSave.SetActive(false);
            buttonLoad.SetActive(false);
            buttonEditGround.SetActive(false);
            textMoney.SetActive(true);
            textDistance.SetActive(true);
            imageFuel.SetActive(true);
            inventoryUI.SetActive(false);

            setAllFantsOpacityAndDragable(1.0f, false);

        }
        else if (mode == MainMode.EDIT)
        {

            //carController.GetComponent<FollowByCamera>().enabled = true;
            //carController.GetComponent<CarController>().enabled = false;
            //carController.GetComponent<Rigidbody2D>().isKinematic = true;
            carController.transform.eulerAngles = new Vector3(0, 0, 0);
            carController.SetActive(false);

            

            Camera.main.GetComponent<BackgroundStatic>().enabled = false;
            Camera.main.GetComponent<PanZoom>().enabled = true;


            backgroundSprite.SetActive(false);
            buttonGas.SetActive(false);
            buttonBrake.SetActive(false);
            buttonReload.SetActive(false);
            buttonDrag.SetActive(true);
            buttonSave.SetActive(true);
            buttonLoad.SetActive(true);
            textMoney.SetActive(false);
            textDistance.SetActive(false);
            imageFuel.SetActive(false);

            groundEditable.SetActive(true);
            groundEditable.GetComponent<EditableGround>().enabled = true;

            buttonEditGround.SetActive(true);

            if (editSubMode == EditSubMode.ADD_FANT)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(true);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                groundEditable.GetComponent<EditableGround>().enableEditing(false);
                //groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                inventoryUI.SetActive(true);
                inventoryUI.GetComponent<UnityEngine.UI.ScrollRect>().horizontalNormalizedPosition = 1.0f;

                setAllFantsOpacityAndDragable(1.0f, true);
            }
            else if (editSubMode == EditSubMode.EDIT_GROUND)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(true);

                groundEditable.GetComponent<EditableGround>().enableEditing(true);
                //groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = new Color(1f, 1f, 1f, 1f);
                inventoryUI.SetActive(false);

                setAllFantsOpacityAndDragable(0.3f, false);
            }
            else if (editSubMode == EditSubMode.NONE)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);

                groundEditable.GetComponent<EditableGround>().enableEditing(false);
                //groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                inventoryUI.SetActive(false);

                setAllFantsOpacityAndDragable(0.3f, false);
            }
        }

    }

    void setAllFantsOpacityAndDragable(float opacity, bool dragable)
    {
        GameObject[] canisters;
        canisters = GameObject.FindGameObjectsWithTag("FuelCanister");
        foreach (var canister in canisters)
        {
            canister.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
            canister.GetComponent<DragableSprite>().enabled = dragable;
        }

        GameObject[] coins;
        coins = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var coin in coins)
        {
            coin.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
            coin.GetComponent<DragableSprite>().enabled = dragable;
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


    void DestroyAllObjects(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }

    public void LoadLevel()
    {

        DestroyAllObjects("Coin");
        DestroyAllObjects("FuelCanister");

        SaveObject saveObject = SaveSystem.Load<SaveObject>("0.txt");

        //LOAD GROUND
        if (saveObject == null)
            Debug.Log("Load retun null");
        else
            groundEditable.GetComponent<EditableGround>().loadPoints(saveObject.splinePoints);

        //LOAD COINS
        Vector3[] coinsPos = saveObject.coinsPositions.ToArray();

        for (int i = 0; i < coinsPos.Length; i++)
        {
            Instantiate(coinPrefab, coinsPos[i], Quaternion.identity);
        }

        //LOAD CANISTERS
        Vector3[] canisterPos = saveObject.canisterPositions.ToArray();

        for (int i = 0; i < canisterPos.Length; i++)
        {
            Instantiate(canisterPrefab, canisterPos[i], Quaternion.identity);
        }
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
