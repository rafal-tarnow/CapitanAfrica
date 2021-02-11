using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using TMPro;



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
        DELETE_FANT,
        NONE
    }
    EditSubMode editSubMode = EditSubMode.NONE;



    private static string SAVE_FOLDER;

    public bool cameraFollowEnable = false;

    public static int coinAmount = 0;

    public GameObject coinPrefab;
    public GameObject canisterPrefab;
    public GameObject metaPrefab;
    public GameObject bombPrefab;
    public GameObject boxPrefab;
    public GameObject board_0Prefab;
    public GameObject board_30Prefab;
    public GameObject board_m30Prefab;

    private GameObject groundEditable;
    private GameObject carController;
    private CarController carControllerScript;
    private GameObject backgroundSprite;

    private GameObject buttonDrag;
    private GameObject buttonEditGround;
    private GameObject buttonTrash;
    private GameObject inventoryUI;

    private GameObject canvasEdit;
    private GameObject canvasPlay;
    private GameObject canvasAdjust;
    private GameObject canvasMeta;

    private AudioSource coinAudioSource;
    private AudioSource musicAudioSource;

    private CoinTextScriptMP coinTextScriptMP;

    float cameraStartupOrthographicSize;

    AdjustManager adjustManager = new AdjustManager();
    DebugManager debugManager = new DebugManager();
    LevelLoader levelLoader;

    void Awake()
    {
        //UnityEngine.Debug.unityLogger.logEnabled = false;


        if (groundEditable == null)
            groundEditable = GameObject.FindWithTag("GroundEditable");

        if (carController == null)
            carController = GameObject.FindWithTag("CarController");

        if (carControllerScript == null)
            carControllerScript = carController.GetComponent<CarController>();

        if (backgroundSprite == null)
            backgroundSprite = GameObject.FindWithTag("BackgroundSprite");

        if (buttonDrag == null)
            buttonDrag = GameObject.FindWithTag("ButtonDrag");

        if (coinTextScriptMP == null)
        {
            coinTextScriptMP = GameObject.FindWithTag("TextMoneyMP").GetComponent<CoinTextScriptMP>();
        }

        if (buttonEditGround == null)
            buttonEditGround = GameObject.FindWithTag("ButtonEditGround");

        if (buttonTrash == null)
            buttonTrash = GameObject.FindWithTag("ButtonTrash");

        if (inventoryUI == null)
            inventoryUI = GameObject.FindWithTag("InventoryUI");

        if (canvasPlay == null)
            canvasPlay = GameObject.FindWithTag("CanvasPlay");

        if (canvasEdit == null)
            canvasEdit = GameObject.FindWithTag("CanvasEdit");

        if (canvasAdjust == null)
            canvasAdjust = GameObject.FindWithTag("CanvasAdjust");

        if (canvasMeta == null)
            canvasMeta = GameObject.FindWithTag("CanvasMeta");

        if (canvasEdit == null)
            canvasEdit = GameObject.FindWithTag("CanvasEdit");

        if ((coinAudioSource == null) || (musicAudioSource == null))
        {
            var audios = this.GetComponents<AudioSource>();
            coinAudioSource = audios[0];
            musicAudioSource = audios[1];
        }

    }

    private void Start()
    {
        cameraStartupOrthographicSize = Camera.main.orthographicSize;

        mode = MainMode.PLAY;
        editSubMode = EditSubMode.NONE;

        if (levelLoader == null)
            levelLoader = new LevelLoader(coinPrefab, canisterPrefab, metaPrefab, bombPrefab, boxPrefab, board_0Prefab, board_30Prefab, board_m30Prefab, groundEditable);

        LoadLevel();

        updateUIState(true);

        coinAmount = PlayerPrefs.GetInt("Coins", 0);
        updateCoinsText();

        //DisableGC();

        adjustManager.LoadAdjustPrefs();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause");
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Coins", coinAmount);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Coins", coinAmount);
        PlayerPrefs.Save();

        //EnableGC();
    }



    public void OnPrefabDropEvent(GameObject prefab)
    {
        UnityEngine.Debug.Log("Game logics, on prefab drop event");
        //Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity);

    }

    public void onBackButtonPressed()
    {
        SceneManager.LoadScene("LevelSelectScene", LoadSceneMode.Single);
    }

    public void onButtonAdjustPressed()
    {
        canvasAdjust.SetActive(true);
    }

    public void onButtonCloseAdjustCanvas()
    {
        canvasAdjust.SetActive(false);
        adjustManager.SaveAdjustPrefs();
    }

    public void restartCurrentScene(bool pressed)
    {
        Physics2D.simulationMode = SimulationMode2D.Update;

        if (!pressed) // if released
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void runNextLevel()
    {
        Physics2D.simulationMode = SimulationMode2D.Update;
#warning 'Check is next level exist to run'
        ScenesVariablePass.levelToRun++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onButtonRelovadLevel()
    {
        LoadLevel();
        updateUIState(true);
    }

    public enum AdjustValue
    {
        Gravity,
        TireFriction,
        GroundFriction,
        SpeedForward,
        TorqueForward,
        SpeedBackward,
        TorqueBackward,
        SpeedFree,
        TorqueFree,
        TyreMass,
        BothDamp,
        BothFreq,
        FrontDamp,
        FrontFreq,
        BackDamp,
        BackFreq,
        RotateWhellsWhenFly,
        CarBodyMass,
        CarBodyFriction,
        CarBodyTorque,
        CarBodyMaxAngVel,
        DriverBodyMass,
        DriverBodyFrequency,
        DriverBodyDamping,
        DriverHeadMass,
        DriverHeadFrequency,
        DriverHeadDamping,
        DriverHeadBreakForce
    }

    class AdjustManager
    {
        class SaveAdjust
        {
            public float Gravity = -9.81f;
            public float TireFriction = 1.0f;
            public float GroundFriction = 1.0f;
            public float SpeedForward = -1400.0f;
            public float TorqueForward = 0.7f;
            public float SpeedBackward = 1400.0f;
            public float TorqueBackward = 0.7f;
            public float SpeedFree = 0.0f;
            public float TorqueFree = 0.08f;
            public float TyreMass = 0.2f;
            public float FrontDamp = 0.7f;
            public float FrontFreq = 2.0f;
            public float BackDamp = 0.7f;
            public float BackFreq = 2.0f;
            public bool RotateWhellsWhenFly = false;
            public float CarBodyMass = 0.08f;
            public float CarBodyFriction = 0.001f;
            public float CarBodyTorque = 500.0f;
            public float CarBodyMaxAngVel = 70.0f;
            public float DriverBodyMass = 0.05f;
            public float DriverBodyFrequency = 110.0f;
            public float DriverBodyDamping = 0.1f;
            public float DriverHeadMass = 0.005f;
            public float DriverHeadFrequency = 3.0f;
            public float DriverHeadDamping = 1.0f;
            public float DriverHeadBreakForce = 10.0f;
        }

        SaveAdjust saveObject;

        public void LoadAdjustPrefs()
        {
            saveObject = SaveSystem.Load<SaveAdjust>(Paths.LEVELS_EDIT + "adjust.json", Paths.TEMPLATES + "template_adjust.txt");


            setAdjustValue(AdjustValue.Gravity, saveObject.Gravity);
            setAdjustValue(AdjustValue.TireFriction, saveObject.TireFriction);
            setAdjustValue(AdjustValue.GroundFriction, saveObject.GroundFriction);
            setAdjustValue(AdjustValue.SpeedForward, saveObject.SpeedForward);
            setAdjustValue(AdjustValue.TorqueForward, saveObject.TorqueForward);
            setAdjustValue(AdjustValue.SpeedBackward, saveObject.SpeedBackward);
            setAdjustValue(AdjustValue.TorqueBackward, saveObject.TorqueBackward);
            setAdjustValue(AdjustValue.SpeedFree, saveObject.SpeedFree);
            setAdjustValue(AdjustValue.TorqueFree, saveObject.TorqueFree);
            setAdjustValue(AdjustValue.TyreMass, saveObject.TyreMass);
            //setAdjustValue(AdjustValue.BothDamp, saveObject.BothDamp);
            //setAdjustValue(AdjustValue.BothFreq, saveObject.BothFreq);
            setAdjustValue(AdjustValue.FrontDamp, saveObject.FrontDamp);
            setAdjustValue(AdjustValue.FrontFreq, saveObject.FrontFreq);
            setAdjustValue(AdjustValue.BackDamp, saveObject.BackDamp);
            setAdjustValue(AdjustValue.BackFreq, saveObject.BackFreq);
            setAdjustValue(AdjustValue.RotateWhellsWhenFly, saveObject.RotateWhellsWhenFly);
            setAdjustValue(AdjustValue.CarBodyMass, saveObject.CarBodyMass);

            setAdjustValue(AdjustValue.CarBodyFriction, saveObject.CarBodyFriction);
            setAdjustValue(AdjustValue.CarBodyTorque, saveObject.CarBodyTorque);
            setAdjustValue(AdjustValue.CarBodyMaxAngVel, saveObject.CarBodyMaxAngVel);

            setAdjustValue(AdjustValue.DriverBodyMass, saveObject.DriverBodyMass);
            setAdjustValue(AdjustValue.DriverBodyFrequency, saveObject.DriverBodyFrequency);
            setAdjustValue(AdjustValue.DriverBodyDamping, saveObject.DriverBodyDamping);
            setAdjustValue(AdjustValue.DriverHeadMass, saveObject.DriverHeadMass);
            setAdjustValue(AdjustValue.DriverHeadFrequency, saveObject.DriverHeadFrequency);
            setAdjustValue(AdjustValue.DriverHeadDamping, saveObject.DriverHeadDamping);
            setAdjustValue(AdjustValue.DriverHeadBreakForce, saveObject.DriverHeadBreakForce);

        }

        public void SaveAdjustPrefs()
        {
            SaveSystem.Save<SaveAdjust>(saveObject, Paths.LEVELS_EDIT + "adjust.json");
        }


        public void setAdjustPrefs(AdjustValue valueName, float value)
        {
            if (valueName == AdjustValue.BothDamp)
            {
                setValueInSaveObject(AdjustValue.FrontDamp, value);
                setValueInSaveObject(AdjustValue.BackDamp, value);
                return;
            }
            if (valueName == AdjustValue.BothFreq)
            {
                setValueInSaveObject(AdjustValue.FrontFreq, value);
                setValueInSaveObject(AdjustValue.BackFreq, value);
                return;
            }
            setValueInSaveObject(valueName, value);
        }

        public void setAdjustPrefs(AdjustValue valueName, bool value)
        {
            setValueInSaveObject(valueName, value);
        }

        public void setValueInSaveObject(AdjustValue valueName, bool value)
        {
            // var propertyInfo = saveObject.GetType().GetProperty(valueName.ToString());
            // if (propertyInfo != null)  //this probably works. Yes it is
            // {
            //     propertyInfo.SetValue(saveObject, value, null);
            // }

            if (valueName.ToString() == "RotateWhellsWhenFly")
                 saveObject.RotateWhellsWhenFly = value;
        }

        public void setValueInSaveObject(AdjustValue valueName, float value)
        {
            // var propertyInfo = saveObject.GetType().GetProperty(valueName.ToString());
            // if (propertyInfo != null)  //this probably works. Yes it is
            // {
            //     propertyInfo.SetValue(saveObject, value, null);
            // }


            if (valueName.ToString() == "Gravity")
                saveObject.Gravity = value;
            else if (valueName.ToString() == "TireFriction")
                saveObject.TireFriction = value;
            else if (valueName.ToString() == "GroundFriction")
                saveObject.GroundFriction = value;
            else if (valueName.ToString() == "SpeedForward")
                saveObject.SpeedForward = value;
            else if (valueName.ToString() == "TorqueForward")
                saveObject.TorqueForward = value;
            else if (valueName.ToString() == "SpeedBackward")
                saveObject.SpeedBackward = value;
            else if (valueName.ToString() == "TorqueBackward")
                saveObject.TorqueBackward = value;
            else if (valueName.ToString() == "SpeedFree")
                saveObject.SpeedFree = value;
            else if (valueName.ToString() == "TorqueFree")
                saveObject.TorqueFree = value;
            else if (valueName.ToString() == "TyreMass")
                saveObject.TyreMass = value;
            else if (valueName.ToString() == "FrontDamp")
                saveObject.FrontDamp = value;
            else if (valueName.ToString() == "FrontFreq")
                saveObject.FrontFreq = value;
            else if (valueName.ToString() == "BackDamp")
                saveObject.BackDamp = value;
            else if (valueName.ToString() == "BackFreq")
                saveObject.BackFreq = value;
            else if (valueName.ToString() == "CarBodyMass")
                saveObject.CarBodyMass = value;
            else if (valueName.ToString() == "CarBodyFriction")
                saveObject.CarBodyFriction = value;
            else if (valueName.ToString() == "CarBodyTorque")
                saveObject.CarBodyTorque = value;
            else if (valueName.ToString() == "CarBodyMaxAngVel")
                saveObject.CarBodyMaxAngVel = value;
            else if (valueName.ToString() == "DriverBodyMass")
                saveObject.DriverBodyMass = value;
            else if (valueName.ToString() == "DriverBodyFrequency")
                saveObject.DriverBodyFrequency = value;
            else if (valueName.ToString() == "DriverBodyDamping")
                saveObject.DriverBodyDamping = value;
            else if (valueName.ToString() == "DriverHeadMass")
                saveObject.DriverHeadMass = value;
            else if (valueName.ToString() == "DriverHeadFrequency")
                saveObject.DriverHeadFrequency = value;
            else if (valueName.ToString() == "DriverHeadDamping")
                saveObject.DriverHeadDamping = value;
            else if (valueName.ToString() == "DriverHeadBreakForce")
                saveObject.DriverHeadBreakForce = value;
        }

        public void setAdjustValue(AdjustValue valueName, float value)
        {
            setAdjustPrefs(valueName, value);
            if (valueName == AdjustValue.Gravity)
            {
                Physics2D.gravity = new Vector2(0, value);
            }
            else if (valueName == AdjustValue.TireFriction)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_FrontTireFriction(value);
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_BackTireFriction(value);
            }
            else if (valueName == AdjustValue.GroundFriction)
            {
                GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial.friction = value;

                GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial = GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial;
            }
            else if (valueName == AdjustValue.SpeedForward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedForward = value;
            }
            else if (valueName == AdjustValue.TorqueForward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueForward = value;
            }
            else if (valueName == AdjustValue.SpeedBackward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedBackward = value;
            }
            else if (valueName == AdjustValue.TorqueBackward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueBackward = value;
            }
            else if (valueName == AdjustValue.SpeedFree)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedFree = value;
            }
            else if (valueName == AdjustValue.TorqueFree)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueFree = value;
            }
            else if (valueName == AdjustValue.TyreMass)
            {
                GameObject.FindGameObjectWithTag("FrontTire").GetComponent<Rigidbody2D>().mass = value;
                GameObject.FindGameObjectWithTag("BackTire").GetComponent<Rigidbody2D>().mass = value;
            }
            else if (valueName == AdjustValue.BothDamp)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_FrontDampingRatio(value);
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_BackDampingRatio(value);
            }
            else if (valueName == AdjustValue.BothFreq)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_FrontFrequency(value);
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_BackFrequency(value);
            }
            else if (valueName == AdjustValue.FrontDamp)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_FrontDampingRatio(value);
            }
            else if (valueName == AdjustValue.FrontFreq)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_FrontFrequency(value);
            }
            else if (valueName == AdjustValue.BackDamp)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_BackDampingRatio(value);
            }
            else if (valueName == AdjustValue.BackFreq)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_BackFrequency(value);
            }
            else if (valueName == AdjustValue.CarBodyMass)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_CarBodyMass(value);
            }
            else if (valueName == AdjustValue.CarBodyFriction)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_CarBodyFriction(value);
            }
            else if (valueName == AdjustValue.CarBodyTorque)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_CarBodyTorque(value);
            }
            else if (valueName == AdjustValue.CarBodyMaxAngVel)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_CarBodyMaxAngVel(value);
            }
            else if (valueName == AdjustValue.DriverBodyMass)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverBodyMass(value);
            }
            else if (valueName == AdjustValue.DriverBodyFrequency)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverBodyFrequency(value);
            }
            else if (valueName == AdjustValue.DriverBodyDamping)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverBodyDamping(value);
            }
            else if (valueName == AdjustValue.DriverHeadMass)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverHeadMass(value);
            }
            else if (valueName == AdjustValue.DriverHeadFrequency)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverHeadFrequency(value);
            }
            else if (valueName == AdjustValue.DriverHeadDamping)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverHeadDamping(value);
            }
            else if (valueName == AdjustValue.DriverHeadBreakForce)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_DriverHeadBreakForce(value);
            }
        }

        public void setAdjustValue(AdjustValue valueName, bool value)
        {
            setAdjustPrefs(valueName, value);
            if (valueName == AdjustValue.RotateWhellsWhenFly)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().setCarParameter_RotateWhellsWhenFly(value);
            }
        }

        public float getFloatAdjustValue(AdjustValue valueName)
        {
            if (valueName == AdjustValue.Gravity)
            {
                return Physics2D.gravity.y;
            }
            else if (valueName == AdjustValue.TireFriction)
            {
                return GameObject.FindGameObjectWithTag("FrontTire").GetComponent<CircleCollider2D>().sharedMaterial.friction;
            }
            else if (valueName == AdjustValue.GroundFriction)
            {
                return GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial.friction;
            }
            else if (valueName == AdjustValue.SpeedForward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedForward;
            }
            else if (valueName == AdjustValue.TorqueForward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueForward;
            }
            else if (valueName == AdjustValue.SpeedBackward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedBackward;
            }
            else if (valueName == AdjustValue.TorqueBackward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueBackward;
            }
            else if (valueName == AdjustValue.SpeedFree)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedFree;
            }
            else if (valueName == AdjustValue.TorqueFree)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueFree;
            }
            else if (valueName == AdjustValue.TyreMass)
            {
                return GameObject.FindGameObjectWithTag("FrontTire").GetComponent<Rigidbody2D>().mass;
            }
            else if (valueName == AdjustValue.BothDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_FrontDampingRatio();
            }
            else if (valueName == AdjustValue.BothFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_FrontFrequency();
            }
            else if (valueName == AdjustValue.FrontDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_FrontDampingRatio();
            }
            else if (valueName == AdjustValue.FrontFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_FrontFrequency();
            }
            else if (valueName == AdjustValue.BackDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_BackDampingRatio();
            }
            else if (valueName == AdjustValue.BackFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_BackFrequency();
            }
            else if (valueName == AdjustValue.CarBodyMass)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_CarBodyMass();
            }
            else if (valueName == AdjustValue.CarBodyFriction)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_CarBodyFriction();
            }
            else if (valueName == AdjustValue.CarBodyTorque)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_CarBodyTorque();
            }
            else if (valueName == AdjustValue.CarBodyMaxAngVel)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_CarBodyMaxAngVel();
            }
            else if (valueName == AdjustValue.DriverBodyMass)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverBodyMass();
            }
            else if (valueName == AdjustValue.DriverBodyFrequency)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverBodyFrequency();
            }
            else if (valueName == AdjustValue.DriverBodyDamping)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverBodyDamping();
            }
            else if (valueName == AdjustValue.DriverHeadMass)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverHeadMass();
            }
            else if (valueName == AdjustValue.DriverHeadFrequency)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverHeadFrequency();
            }
            else if (valueName == AdjustValue.DriverHeadDamping)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverHeadDamping();
            }
            else if (valueName == AdjustValue.DriverHeadBreakForce)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_DriverHeadBreakForce();
            }
            return 0.0f;
        }

        public bool getBoolAdjustValue(AdjustValue valueName)
        {
            if (valueName == AdjustValue.RotateWhellsWhenFly)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().getCarParameter_RotateWhellsWhenFly();
            }

            return false;
        }
    }


    public void setAdjustValue(AdjustValue valueName, float value)
    {
        adjustManager.setAdjustValue(valueName, value);
    }

    public void setAdjustValue(AdjustValue valueName, bool value)
    {
        adjustManager.setAdjustValue(valueName, value);
    }


    public float getFloatAdjustValue(AdjustValue valueName)
    {
        return adjustManager.getFloatAdjustValue(valueName);
    }

    public bool getBoolAdjustValue(AdjustValue valueName)
    {
        return adjustManager.getBoolAdjustValue(valueName);
    }


    public void OnUIInventoryDragEvent(bool isDragging)
    {
        //Camera.main.GetComponent<PanZoom>().enabled = !isDragging; //if inventory is dragging disable pan zoom
    }


    public void OnCoinTriggerEnter2D(GameObject coin, Collider2D collider)
    {
        Destroy(coin);
        //Debug.Log("GameLogics::OnCoinTriggerEnter2D");
        coinAmount++;

        coinAudioSource.Play();
        updateCoinsText();
    }


    private void updateCoinsText()
    {
        coinTextScriptMP.setCoins(coinAmount);
    }


    public void OnPanZoomActiveEvent(bool active)
    {
        //Debug.Log ("Game Logics OnPanZoomActiveEvent = " + active);
        if (active)
            setAllFants_Dragable(false);
        else
            updateUIState(false);

        //     if(active)
        //         setAllFants_Dragable(false);
        //     else
        //     {
        //         if((mode == MainMode.EDIT) && (editSubMode == EditSubMode.NONE))

        //         if((mode == MainMode.EDIT) && (editSubMode == EditSubMode.NONE))

        //         if((mode == MainMode.EDIT) && (editSubMode == EditSubMode.NONE))



        //    ADD_FANT,
        //     EDIT_GROUND,
        //     DELETE_FANT,
        //     NONE            updateUIState();
    }

    public void onEditButton()
    {
        mode = MainMode.EDIT;
        LoadLevel();
        updateUIState(true);
    }


    public void onPlayButton()
    {
        mode = MainMode.PLAY;
        SaveLevel();
        updateUIState(true);
        carController.transform.position = Camera.main.transform.position;
    }

    public void onEditGroundButton(bool state)
    {
        if (state)
        {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.EDIT_GROUND))
            {
                editSubMode = EditSubMode.EDIT_GROUND;
                updateUIState(true);
            }
        }
        else
        {
            editSubMode = EditSubMode.NONE;
            updateUIState(true);
        }
    }

    public void onTrashButton(bool state)
    {
        if (state)
        {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.DELETE_FANT))
            {
                editSubMode = EditSubMode.DELETE_FANT;
                updateUIState(true);
            }
        }
        else
        {
            editSubMode = EditSubMode.NONE;
            updateUIState(true);
        }
    }


    public void onGasButton(bool state)
    {
        carControllerScript.onGasPedalPressedEvent(state);
    }


    public void onBrakeButton(bool state)
    {
        carControllerScript.onBrakePedalPressedEvent(state);
    }


    public void onDragButton(bool state)
    {
        if (state)
        {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.ADD_FANT))
            {
                editSubMode = EditSubMode.ADD_FANT;
                updateUIState(true);
            }
        }
        else
        {
            editSubMode = EditSubMode.NONE;
            updateUIState(true);
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

    public void onMetaReached()
    {
        unlockNextLevel();
        canvasMeta.SetActive(true);

        // if (Physics2D.simulationMode == SimulationMode2D.Script)
        // {
        //     Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        //     Time.timeScale = 0;
        // }
        Physics2D.simulationMode = SimulationMode2D.Script;
    }


    private void unlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevelIndex", 0);

        if (unlockedLevel < ScenesVariablePass.levelToRun + 1)
        {
            PlayerPrefs.SetInt("unlockedLevelIndex", ScenesVariablePass.levelToRun + 1);
            PlayerPrefs.Save();
        }
    }


    private void updateUIState(bool alignInventory)
    {
        if (mode == MainMode.PLAY)
        {
            musicAudioSource.Play();

            backgroundSprite.SetActive(true);
            Camera.main.GetComponent<BackgroundStatic>().enabled = true;
            Camera.main.GetComponent<PanZoom>().enabled = false;
            Camera.main.orthographicSize = cameraStartupOrthographicSize;

            groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.PLAY);

            carController.SetActive(true);
            // carController.GetComponent<FollowByCamera>().enabled = false;
            // carController.GetComponent<CarController>().enabled = true;

            //CANVAS
            canvasEdit.SetActive(false);
            canvasPlay.SetActive(true);
            canvasAdjust.SetActive(false);
            canvasMeta.SetActive(false);


            setAllFants_Opacity_Dragable_Deletable(1.0f, false, false);

            Physics2D.simulationMode = SimulationMode2D.Update;

        }
        else if (mode == MainMode.EDIT)
        {
            musicAudioSource.Pause();

            //carController.GetComponent<FollowByCamera>().enabled = true;
            //carController.GetComponent<CarController>().enabled = false;
            //carController.GetComponent<Rigidbody2D>().isKinematic = true;
            carController.transform.eulerAngles = new Vector3(0, 0, 0);
            carController.SetActive(false);

            Camera.main.GetComponent<BackgroundStatic>().enabled = false;
            Camera.main.GetComponent<PanZoom>().enabled = true;

            backgroundSprite.SetActive(false);

            canvasPlay.SetActive(false);
            canvasEdit.SetActive(true);
            canvasAdjust.SetActive(false);
            canvasMeta.SetActive(false);

            if (editSubMode == EditSubMode.ADD_FANT)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(true);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonTrash.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_INACTIVE);

                inventoryUI.SetActive(true);
                if (alignInventory)
                    inventoryUI.GetComponent<UnityEngine.UI.ScrollRect>().horizontalNormalizedPosition = 1.0f;

                setAllFants_Opacity_Dragable_Deletable(1.0f, true, false);
            }
            else if (editSubMode == EditSubMode.EDIT_GROUND)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(true);
                buttonTrash.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_DRAG_ADD_POINTS);

                inventoryUI.SetActive(false);

                setAllFants_Opacity_Dragable_Deletable(0.3f, false, false);
            }
            else if (editSubMode == EditSubMode.DELETE_FANT)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonTrash.GetComponent<ButtonBistable>().SetStateWithoutEvent(true);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_DELETE_POINTS);

                inventoryUI.SetActive(false);

                setAllFants_Opacity_Dragable_Deletable(1.0f, false, true);
            }
            else if (editSubMode == EditSubMode.NONE)
            {
                buttonDrag.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonEditGround.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);
                buttonTrash.GetComponent<ButtonBistable>().SetStateWithoutEvent(false);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_INACTIVE);

                inventoryUI.SetActive(false);

                setAllFants_Opacity_Dragable_Deletable(0.3f, false, false);
            }
        }
    }

    void setAllFants_Dragable(bool dragable)
    {
        UnityEngine.Debug.Log("GameLogics::setAllFantsDragable = " + dragable);
        setGivenTagFantsDragable("FuelCanister", dragable);
        setGivenTagFantsDragable("Coin", dragable);
        setGivenTagFantsDragable("Meta", dragable);
        setGivenTagFantsDragable("Bomb", dragable);
        setGivenTagFantsDragable("Box", dragable);
        setGivenTagFantsDragable("Board_0", dragable);
        setGivenTagFantsDragable("Board_30", dragable);
        setGivenTagFantsDragable("Board_m30", dragable);
    }

    void setAllFants_Opacity_Dragable_Deletable(float opacity, bool dragable, bool deletable)
    {
        setGivenTagFantsOpacityAndDragable("FuelCanister", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Coin", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Meta", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Bomb", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Box", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Board_0", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Board_30", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable("Board_m30", opacity, dragable, deletable);

    }

    void setGivenTagFantsDragable(string tag, bool dragable)
    {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag(tag);
        foreach (var fant in fants)
        {
            if (dragable == false)
                Destroy(fant.GetComponent<DragableSprite>());
            else
                fant.AddComponent<DragableSprite>();

        }
    }

    void setGivenTagFantsOpacityAndDragable(string tag, float opacity, bool dragable, bool deletable)
    {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag(tag);
        foreach (var fant in fants)
        {
            fant.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);

            if (dragable == false)
                Destroy(fant.GetComponent<DragableSprite>());
            else
                fant.AddComponent<DragableSprite>();

            if (deletable == false)
                Destroy(fant.GetComponent<TouchDeleteSprite>());
            else
                fant.AddComponent<TouchDeleteSprite>();
        }
    }

    public void SaveLevel()
    {
        levelLoader.SaveLevel(getCurrentLevelFileName());
    }

    private void loadFantsPositionsToSaveObjectByTag(ref SaveObject saveObject, string tag)
    {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag(tag);
        saveObject.fantPositions[tag] = new List<Vector3>();

        foreach (var fant in fants)
        {
            saveObject.fantPositions[tag].Add(fant.transform.position);
        }
    }


    string getCurrentLevelFileName()
    {
        int levelIndex = ScenesVariablePass.levelToRun;
        string levelFileName = levelIndex.ToString() + ".txt";
        return levelFileName;
    }


    public void LoadLevel()
    {
        levelLoader.LoadLevel(getCurrentLevelFileName());
    }


    static void EnableGC()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        // Trigger a collection to free memory.
        GC.Collect();
    }

    static void DisableGC()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    //  private void FixedUpdate() 
    //  {
    //     debugManager.FixedUpdate();
    // }   

    void Update()
    {
        //Debug.Log("GameLogics Update");
        // Debug.Log("GameLogics " + Time.time.ToString());
    }

    void LateUpdate()
    {

    }



}


