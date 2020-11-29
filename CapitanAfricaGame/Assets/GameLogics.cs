using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using TMPro;



public class GameLogics : MonoBehaviour {

    enum MainMode {
        PLAY,
        EDIT,
    }
    MainMode mode = MainMode.PLAY;

    enum EditSubMode {
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



    private CoinTextScriptMP coinTextScriptMP;

    float cameraStartupOrthographicSize;

    AdjustManager adjustManager = new AdjustManager();
    DebugManager debugManager = new DebugManager();

    void Awake () {
        //Application.targetFrameRate = 62;
        UnityEngine.Debug.unityLogger.logEnabled = false;

        SaveSystem.Init ();

        if (groundEditable == null)
            groundEditable = GameObject.FindWithTag ("GroundEditable");

        if (carController == null)
            carController = GameObject.FindWithTag ("CarController");

        if(carControllerScript == null)
            carControllerScript = carController.GetComponent<CarController>();

        if (backgroundSprite == null)
            backgroundSprite = GameObject.FindWithTag ("BackgroundSprite");

        if (buttonDrag == null)
            buttonDrag = GameObject.FindWithTag ("ButtonDrag");

        if (coinTextScriptMP == null)
        {
            coinTextScriptMP = GameObject.FindWithTag("TextMoneyMP").GetComponent<CoinTextScriptMP>();
        }

        if (buttonEditGround == null)
            buttonEditGround = GameObject.FindWithTag ("ButtonEditGround");

        if (buttonTrash == null)
            buttonTrash = GameObject.FindWithTag ("ButtonTrash");

        if (inventoryUI == null)
            inventoryUI = GameObject.FindWithTag ("InventoryUI");

        if(canvasPlay == null)
            canvasPlay = GameObject.FindWithTag("CanvasPlay");

        if(canvasEdit == null)
            canvasEdit = GameObject.FindWithTag("CanvasEdit");

        if(canvasAdjust == null)
            canvasAdjust = GameObject.FindWithTag("CanvasAdjust");

    }

    private void Start () {
        cameraStartupOrthographicSize = Camera.main.orthographicSize;

        mode = MainMode.PLAY;
        editSubMode = EditSubMode.NONE;

        LoadLevel ();
        updateUIState (true);

        coinAmount = PlayerPrefs.GetInt("Coins",0);
        updateCoinsText();

        //DisableGC();

        adjustManager.LoadAdjustPrefs();
        debugManager.Start();
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

    private void OnDestroy() {
        PlayerPrefs.SetInt("Coins", coinAmount);
        PlayerPrefs.Save();  

        //EnableGC();
    }



    public void OnPrefabDropEvent (GameObject prefab) {
        UnityEngine.Debug.Log ("Game logics, on prefab drop event");
        //Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity);

    }

    public void onBackButtonPressed () {
        SceneManager.LoadScene ("LevelSelectScene", LoadSceneMode.Single);
    }

    public void onButtonAdjustPressed()
    {
        canvasAdjust.SetActive(true);
    }

    public void onButtonCloseAdjustCanvas()
    {
        canvasAdjust.SetActive(false);
    }

    public void onReloadButtonReleased (bool pressed) {
        if (!pressed) // if released
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void onButtonRelovadLevel () {
        LoadLevel ();
        updateUIState (true);
    }

    public enum AdjustValue{
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
        CarBodyMass
    }
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
    class AdjustManager{
        public void LoadAdjustPrefs()
        {
            setAdjustValue(AdjustValue.Gravity, PlayerPrefs.GetFloat(AdjustValue.Gravity.ToString(),-9.81f));
            setAdjustValue(AdjustValue.TireFriction, PlayerPrefs.GetFloat(AdjustValue.TireFriction.ToString(),1.0f));
            setAdjustValue(AdjustValue.GroundFriction, PlayerPrefs.GetFloat(AdjustValue.GroundFriction.ToString(),1.0f));
            setAdjustValue(AdjustValue.SpeedForward, PlayerPrefs.GetFloat(AdjustValue.SpeedForward.ToString(),-1400.0f));
            setAdjustValue(AdjustValue.TorqueForward, PlayerPrefs.GetFloat(AdjustValue.TorqueForward.ToString(),0.7f));
            setAdjustValue(AdjustValue.SpeedBackward, PlayerPrefs.GetFloat(AdjustValue.SpeedBackward.ToString(),1400.0f));
            setAdjustValue(AdjustValue.TorqueBackward, PlayerPrefs.GetFloat(AdjustValue.TorqueBackward.ToString(),0.7f));
            setAdjustValue(AdjustValue.SpeedFree, PlayerPrefs.GetFloat(AdjustValue.SpeedFree.ToString(),0.0f));
            setAdjustValue(AdjustValue.TorqueFree, PlayerPrefs.GetFloat(AdjustValue.TorqueFree.ToString(),0.08f));
            setAdjustValue(AdjustValue.TyreMass, PlayerPrefs.GetFloat(AdjustValue.TyreMass.ToString(),0.2f));
            //setAdjustValue(AdjustValue.BothDamp, PlayerPrefs.GetFloat(AdjustValue.BothDamp.ToString(),0.7f));
            //setAdjustValue(AdjustValue.BothFreq, PlayerPrefs.GetFloat(AdjustValue.BothFreq.ToString(),2.0f));
            setAdjustValue(AdjustValue.FrontDamp, PlayerPrefs.GetFloat(AdjustValue.FrontDamp.ToString(),0.7f));
            setAdjustValue(AdjustValue.FrontFreq, PlayerPrefs.GetFloat(AdjustValue.FrontFreq.ToString(),2.0f));
            setAdjustValue(AdjustValue.BackDamp, PlayerPrefs.GetFloat(AdjustValue.BackDamp.ToString(),0.7f));
            setAdjustValue(AdjustValue.BackFreq, PlayerPrefs.GetFloat(AdjustValue.BackFreq.ToString(),2.0f));
            setAdjustValue(AdjustValue.CarBodyMass, PlayerPrefs.GetFloat(AdjustValue.CarBodyMass.ToString(),0.08f));
        }

        public void SaveAdjustPrefs(AdjustValue valueName, float value)
        {
            if(valueName == AdjustValue.BothDamp)
            {
                PlayerPrefs.SetFloat(AdjustValue.FrontDamp.ToString(), value);
                PlayerPrefs.SetFloat(AdjustValue.BackDamp.ToString(), value);
                return;
            }
            if(valueName == AdjustValue.BothFreq)
            {
                PlayerPrefs.SetFloat(AdjustValue.FrontFreq.ToString(), value);
                PlayerPrefs.SetFloat(AdjustValue.BackFreq.ToString(), value);
                return;
            }
            PlayerPrefs.SetFloat(valueName.ToString(), value);
        }

        public void setAdjustValue(AdjustValue valueName, float value)
        {
            SaveAdjustPrefs(valueName, value);
            if(valueName == AdjustValue.Gravity)
            {
                Physics2D.gravity = new Vector2(0, value);
            }
            else if(valueName == AdjustValue.TireFriction)
            {
                GameObject.FindGameObjectWithTag("FrontTire").GetComponent<CircleCollider2D>().sharedMaterial.friction = value;
                GameObject.FindGameObjectWithTag("BackTire").GetComponent<CircleCollider2D>().sharedMaterial.friction = value;

                GameObject.FindGameObjectWithTag("FrontTire").GetComponent<CircleCollider2D>().sharedMaterial = GameObject.FindGameObjectWithTag("FrontTire").GetComponent<CircleCollider2D>().sharedMaterial;
                GameObject.FindGameObjectWithTag("BackTire").GetComponent<CircleCollider2D>().sharedMaterial = GameObject.FindGameObjectWithTag("BackTire").GetComponent<CircleCollider2D>().sharedMaterial;
            }
            else if(valueName == AdjustValue.GroundFriction)
            {
                GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial.friction = value;

                GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial = GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial;
            }
            else if(valueName == AdjustValue.SpeedForward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedForward = value;
            }
            else if(valueName == AdjustValue.TorqueForward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueForward = value;
            }
            else if(valueName == AdjustValue.SpeedBackward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedBackward = value;
            }
            else if(valueName == AdjustValue.TorqueBackward)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueBackward = value;
            }
            else if(valueName == AdjustValue.SpeedFree)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedFree = value;
            }
            else if(valueName == AdjustValue.TorqueFree)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueFree = value;
            }
            else if(valueName == AdjustValue.TyreMass)
            {
                GameObject.FindGameObjectWithTag("FrontTire").GetComponent<Rigidbody2D>().mass = value;
                GameObject.FindGameObjectWithTag("BackTire").GetComponent<Rigidbody2D>().mass = value;
            }
            else if(valueName == AdjustValue.BothDamp)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                foreach (WheelJoint2D joint in joints)
                {
                    suspension = joint.suspension;
                    suspension.dampingRatio = value;
                    joint.suspension = suspension;
                }
            }
            else if(valueName == AdjustValue.BothFreq)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                foreach (WheelJoint2D joint in joints)
                {
                    suspension = joint.suspension;
                    suspension.frequency = value;
                    joint.suspension = suspension;
                }
            }
            else if(valueName == AdjustValue.FrontDamp)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                
                suspension = joints[0].suspension;
                suspension.dampingRatio = value;
                joints[0].suspension = suspension;
                
            }
            else if(valueName == AdjustValue.FrontFreq)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                
                suspension = joints[0].suspension;
                suspension.frequency = value;
                joints[0].suspension = suspension;
            }
            else if(valueName == AdjustValue.BackDamp)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                
                suspension = joints[1].suspension;
                suspension.dampingRatio = value;
                joints[1].suspension = suspension;
                
            }
            else if(valueName == AdjustValue.BackFreq)
            {
                WheelJoint2D[] joints;
                JointSuspension2D suspension;
                joints = GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>();
                
                suspension = joints[1].suspension;
                suspension.frequency = value;
                joints[1].suspension = suspension;
            }
            else if(valueName == AdjustValue.CarBodyMass)
            {
                GameObject.FindGameObjectWithTag("CarController").GetComponent<Rigidbody2D>().mass = value;
            }
        }

        public float getAdjustValue(AdjustValue valueName)
        {
            if(valueName == AdjustValue.Gravity)
            {
                return Physics2D.gravity.y;
            }
            else if(valueName == AdjustValue.TireFriction)
            {
                return GameObject.FindGameObjectWithTag("FrontTire").GetComponent<CircleCollider2D>().sharedMaterial.friction;
            }
            else if(valueName == AdjustValue.GroundFriction)
            {
                return GameObject.FindGameObjectWithTag("GroundEditable").GetComponent<EdgeCollider2D>().sharedMaterial.friction;
            }
            else if(valueName == AdjustValue.SpeedForward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedForward;
            }
            else if(valueName == AdjustValue.TorqueForward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueForward;
            }
            else if(valueName == AdjustValue.SpeedBackward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedBackward;
            }
            else if(valueName == AdjustValue.TorqueBackward)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueBackward;
            }
            else if(valueName == AdjustValue.SpeedFree)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().speedFree;
            }
            else if(valueName == AdjustValue.TorqueFree)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<CarController>().torqueFree;
            }
            else if(valueName == AdjustValue.TyreMass)
            {
                return GameObject.FindGameObjectWithTag("FrontTire").GetComponent<Rigidbody2D>().mass;
            }
            else if(valueName == AdjustValue.BothDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<WheelJoint2D>().suspension.dampingRatio;
            }
            else if(valueName == AdjustValue.BothFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<WheelJoint2D>().suspension.frequency;
            }
            else if(valueName == AdjustValue.FrontDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>()[0].suspension.dampingRatio;
            }
            else if(valueName == AdjustValue.FrontFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>()[0].suspension.frequency;
            }
            else if(valueName == AdjustValue.BackDamp)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>()[1].suspension.dampingRatio;
            }
            else if(valueName == AdjustValue.BackFreq)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponents<WheelJoint2D>()[1].suspension.frequency;
            }
            else if(valueName == AdjustValue.CarBodyMass)
            {
                return GameObject.FindGameObjectWithTag("CarController").GetComponent<Rigidbody2D>().mass;
            }
            return 0.0f;
        }
    }
    public void setAdjustValue(AdjustValue valueName, float value)
    {
        adjustManager.setAdjustValue(valueName, value);
    }

    public float getAdjustValue(AdjustValue valueName)
    {
        return adjustManager.getAdjustValue(valueName);
    }

    public void OnUIInventoryDragEvent (bool isDragging) {
        //Camera.main.GetComponent<PanZoom>().enabled = !isDragging; //if inventory is dragging disable pan zoom
    }
    public void OnDragRedDotEvent (bool isDragging) {
        //Camera.main.GetComponent<PanZoom>().enabled = !isDragging; //if red dot is dragging disable pan zoom
    }

    public void OnDragSprite (bool isDragging) {
        //Camera.main.GetComponent<PanZoom>().enabled = !isDragging;
    }

    public void OnCoinTriggerEnter2D(GameObject coin, Collider2D collider)
    {
        Destroy(coin);     
        //Debug.Log("GameLogics::OnCoinTriggerEnter2D");
        coinAmount++;
        updateCoinsText();
    }

    private void updateCoinsText()
    {
        coinTextScriptMP.setCoins(coinAmount);
    }

    public void OnPanZoomActiveEvent (bool active) {
        //Debug.Log ("Game Logics OnPanZoomActiveEvent = " + active);
        if(active)
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

    public void onEditButton () {
        mode = MainMode.EDIT;
        LoadLevel ();
        updateUIState (true);
    }

    public void onPlayButton () {
        mode = MainMode.PLAY;
        SaveLevel ();
        updateUIState (true);
        carController.transform.position = Camera.main.transform.position;
    }

    public void onEditGroundButton (bool state) {
        if (state) {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.EDIT_GROUND)) {
                editSubMode = EditSubMode.EDIT_GROUND;
                updateUIState (true);
            }
        } else {
            editSubMode = EditSubMode.NONE;
            updateUIState (true);
        }
    }

    public void onTrashButton (bool state) {
        if (state) {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.DELETE_FANT)) {
                editSubMode = EditSubMode.DELETE_FANT;
                updateUIState (true);
            }
        } else {
            editSubMode = EditSubMode.NONE;
            updateUIState (true);
        }
    }


    bool gassPressed = false;
    bool brakePressed = false;

    public void onGasButton(bool state)
    {
        gassPressed = state;

        if(state && brakePressed)
        {
            carControllerScript.setGas(1.0f);
            return;
        }
        if(state && !brakePressed)
        {
            carControllerScript.setGas(1.0f);
            return;
        }
        if(!state && brakePressed)
        {
            carControllerScript.setGas(-1.0f);
            return;
        }
        if(!state && !brakePressed)
        {
            carControllerScript.setGas(0.0f);
            return;
        }

    }

    public void onBrakeButton(bool state)
    {
        brakePressed = state;

        if(state && gassPressed)
        {
            carControllerScript.setGas(-1.0f);
            return;
        }
        if(state && !gassPressed)
        {
            carControllerScript.setGas(-1.0f);
            return;
        }
        if(!state && gassPressed)
        {
            carControllerScript.setGas(1.0f);
            return;
        }
        if(!state && !gassPressed)
        {
            carControllerScript.setGas(0.0f);
            return;
        }
    }

    public void onDragButton (bool state) {
        if (state) {
            if ((mode == MainMode.EDIT) && (editSubMode != EditSubMode.ADD_FANT)) {
                editSubMode = EditSubMode.ADD_FANT;
                updateUIState (true);
            }
        } else {
            editSubMode = EditSubMode.NONE;
            updateUIState (true);
        }
    }

    public void onPanZoomButton (bool state) {
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

    public void onMetaReached () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void updateUIState (bool alignInventory) {
        if (mode == MainMode.PLAY) {

            backgroundSprite.SetActive (true);
            Camera.main.GetComponent<BackgroundStatic> ().enabled = true;
            Camera.main.GetComponent<PanZoom> ().enabled = false;
            Camera.main.orthographicSize = cameraStartupOrthographicSize;

            groundEditable.SetActive (true);
            groundEditable.GetComponent<EditableGround> ().enabled = false; // in play mode edition is disabled

            carController.SetActive (true);
            // carController.GetComponent<FollowByCamera>().enabled = false;
            // carController.GetComponent<CarController>().enabled = true;

            //CANVAS
            canvasEdit.SetActive(false);
            canvasPlay.SetActive(true);
            canvasAdjust.SetActive(false);

            groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1.0f);
            setAllFants_Opacity_Dragable_Deletable (1.0f, false, false);


        } else if (mode == MainMode.EDIT) {

            //carController.GetComponent<FollowByCamera>().enabled = true;
            //carController.GetComponent<CarController>().enabled = false;
            //carController.GetComponent<Rigidbody2D>().isKinematic = true;
            carController.transform.eulerAngles = new Vector3 (0, 0, 0);
            carController.SetActive (false);

            Camera.main.GetComponent<BackgroundStatic> ().enabled = false;
            Camera.main.GetComponent<PanZoom> ().enabled = true;

            backgroundSprite.SetActive (false);

            groundEditable.SetActive (true);
            groundEditable.GetComponent<EditableGround> ().enabled = true;

            canvasPlay.SetActive(false);
            canvasEdit.SetActive(true);
            canvasAdjust.SetActive(false);

            if (editSubMode == EditSubMode.ADD_FANT) {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);

                groundEditable.GetComponent<EditableGround> ().enableEditing (false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1.0f);
                inventoryUI.SetActive (true);
                if(alignInventory)
                    inventoryUI.GetComponent<UnityEngine.UI.ScrollRect> ().horizontalNormalizedPosition = 1.0f;
                
                setAllFants_Opacity_Dragable_Deletable (1.0f, true, false);
            } else if (editSubMode == EditSubMode.EDIT_GROUND) {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);

                groundEditable.GetComponent<EditableGround> ().enableEditing (true);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1f);
                inventoryUI.SetActive (false);

                setAllFants_Opacity_Dragable_Deletable (0.3f, false, false);
            } else if (editSubMode == EditSubMode.DELETE_FANT) {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);

                groundEditable.GetComponent<EditableGround> ().enableEditing (false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 0.8f);
                inventoryUI.SetActive (false);

                setAllFants_Opacity_Dragable_Deletable (1.0f, false, true);
            } else if (editSubMode == EditSubMode.NONE) {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);

                groundEditable.GetComponent<EditableGround> ().enableEditing (false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1.0f);
                inventoryUI.SetActive (false);

                setAllFants_Opacity_Dragable_Deletable (0.3f, false, false);
            }
        }

    }

    void setAllFants_Dragable (bool dragable) {
        UnityEngine.Debug.Log("GameLogics::setAllFantsDragable = " + dragable);
        setGivenTagFantsDragable ("FuelCanister", dragable);
        setGivenTagFantsDragable ("Coin", dragable);
        setGivenTagFantsDragable ("Meta", dragable);
        setGivenTagFantsDragable ("Bomb", dragable);
        setGivenTagFantsDragable ("Box", dragable);
        setGivenTagFantsDragable ("Board_0", dragable);
        setGivenTagFantsDragable ("Board_30", dragable);
        setGivenTagFantsDragable ("Board_m30", dragable);
    }

    void setAllFants_Opacity_Dragable_Deletable (float opacity, bool dragable, bool deletable) {
        setGivenTagFantsOpacityAndDragable ("FuelCanister", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Coin", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Meta", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Bomb", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Box", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Board_0", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Board_30", opacity, dragable, deletable);
        setGivenTagFantsOpacityAndDragable ("Board_m30", opacity, dragable, deletable);

    }

    void setGivenTagFantsDragable (string tag, bool dragable) {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag (tag);
        foreach (var fant in fants) {
            if (dragable == false)
                Destroy (fant.GetComponent<DragableSprite> ());
            else
                fant.AddComponent<DragableSprite> ();

        }
    }

    void setGivenTagFantsOpacityAndDragable (string tag, float opacity, bool dragable, bool deletable) {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag (tag);
        foreach (var fant in fants) {
            fant.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, opacity);

            if (dragable == false)
                Destroy (fant.GetComponent<DragableSprite> ());
            else
                fant.AddComponent<DragableSprite> ();

            if (deletable == false)
                Destroy (fant.GetComponent<TouchDeleteSprite> ());
            else
                fant.AddComponent<TouchDeleteSprite> ();
        }
    }

    public void SaveLevel () {

        SaveObject saveObject = new SaveObject ();
        saveObject.fantPositions = new Dictionary<string, List<Vector3>> ();

        //SAVE GROUND
        saveObject.splinePoints = groundEditable.GetComponent<EditableGround> ().getSplinesPointsPositions ();

        // loadFantsPositionsToSaveObjectByTag(ref saveObject, "Coin");
        // loadFantsPositionsToSaveObjectByTag(ref saveObject, "FuelCanister");
        // loadFantsPositionsToSaveObjectByTag(ref saveObject, "Meta");

        //SAVE COINS
        GameObject[] coins;
        coins = GameObject.FindGameObjectsWithTag ("Coin");
        saveObject.coinsPositions = new List<Vector3> ();

        foreach (var coin in coins) {
            saveObject.coinsPositions.Add (coin.transform.position);
        }

        //SAVE CANISTERS
        GameObject[] canisters;
        canisters = GameObject.FindGameObjectsWithTag ("FuelCanister");
        saveObject.canisterPositions = new List<Vector3> ();

        foreach (var canister in canisters) {
            saveObject.canisterPositions.Add (canister.transform.position);
        }

        //SAVE METAS
        GameObject[] metas;
        metas = GameObject.FindGameObjectsWithTag ("Meta");
        saveObject.metaPositions = new List<Vector3> ();

        foreach (var meta in metas) {
            saveObject.metaPositions.Add (meta.transform.position);
        }

        //SAVE Bombs
        GameObject[] bombs;
        bombs = GameObject.FindGameObjectsWithTag ("Bomb");
        saveObject.bombPositions = new List<Vector3> ();
        saveObject.bombAngles = new List<Vector3> ();

        foreach (var bomb in bombs) {
            saveObject.bombPositions.Add (bomb.transform.position);
            saveObject.bombAngles.Add (bomb.transform.rotation.eulerAngles);
        }

        //SAVE Box
        GameObject[] boxes;
        boxes = GameObject.FindGameObjectsWithTag ("Box");
        saveObject.boxPositions = new List<Vector3> ();
        saveObject.boxAngles = new List<Vector3> ();

        foreach (var box in boxes) {
            saveObject.boxPositions.Add (box.transform.position);
            saveObject.boxAngles.Add (box.transform.rotation.eulerAngles);
        }

        //SAVE Board_0
        GameObject[] boards_0;
        boards_0 = GameObject.FindGameObjectsWithTag ("Board_0");
        saveObject.board_0Positions = new List<Vector3> ();
        saveObject.board_0Angles = new List<Vector3> ();

        foreach (var board in boards_0) {
            saveObject.board_0Positions.Add (board.transform.position);
            saveObject.board_0Angles.Add (board.transform.rotation.eulerAngles);
        }

        //SAVE Board_30
        GameObject[] boards_30;
        boards_30 = GameObject.FindGameObjectsWithTag ("Board_30");
        saveObject.board_30Positions = new List<Vector3> ();
        saveObject.board_30Angles = new List<Vector3> ();

        foreach (var board in boards_30) {
            saveObject.board_30Positions.Add (board.transform.position);
            saveObject.board_30Angles.Add (board.transform.rotation.eulerAngles);
        }

        //SAVE Board_m30
        GameObject[] boards_m30;
        boards_m30 = GameObject.FindGameObjectsWithTag ("Board_m30");
        saveObject.board_m30Positions = new List<Vector3> ();
        saveObject.board_m30Angles = new List<Vector3> ();

        foreach (var board in boards_m30) {
            saveObject.board_m30Positions.Add (board.transform.position);
            saveObject.board_m30Angles.Add (board.transform.rotation.eulerAngles);
        }

        SaveSystem.Save<SaveObject> (saveObject, "0.txt");
    }

    private void loadFantsPositionsToSaveObjectByTag (ref SaveObject saveObject, string tag) {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag (tag);
        saveObject.fantPositions[tag] = new List<Vector3> ();

        foreach (var fant in fants) {
            saveObject.fantPositions[tag].Add (fant.transform.position);
        }
    }

    void DestroyAllObjects (string tag) {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);

        for (var i = 0; i < gameObjects.Length; i++) {
            Destroy (gameObjects[i]);
        }
    }

    public void LoadLevel () {

        DestroyAllObjects ("Coin");
        DestroyAllObjects ("FuelCanister");
        DestroyAllObjects ("Meta");
        DestroyAllObjects ("Box");
        DestroyAllObjects ("Bomb");
        DestroyAllObjects ("Board_0");
        DestroyAllObjects ("Board_30");
        DestroyAllObjects ("Board_m30");

        SaveObject saveObject = SaveSystem.Load<SaveObject> ("0.txt");

        //LOAD GROUND
        if (saveObject == null)
            UnityEngine.Debug.Log ("Load retun null");
        else
            groundEditable.GetComponent<EditableGround> ().loadPoints (saveObject.splinePoints);

        //LOAD COINS
        Vector3[] coinsPos = saveObject.coinsPositions.ToArray ();

        for (int i = 0; i < coinsPos.Length; i++) {
            Instantiate (coinPrefab, coinsPos[i], Quaternion.identity);
        }

        //LOAD CANISTERS
        Vector3[] canisterPos = saveObject.canisterPositions.ToArray ();

        for (int i = 0; i < canisterPos.Length; i++) {
            Instantiate (canisterPrefab, canisterPos[i], Quaternion.identity);
        }

        //LOAD META
        Vector3[] metasPos = saveObject.metaPositions.ToArray ();

        for (int i = 0; i < metasPos.Length; i++) {
            Instantiate (metaPrefab, metasPos[i], Quaternion.identity);
        }

        //LOAD Bombs
        Vector3[] bombsPos = saveObject.bombPositions.ToArray ();
        Vector3[] bombsRot = saveObject.bombAngles.ToArray ();

        for (int i = 0; i < bombsPos.Length; i++) {
            Instantiate (bombPrefab, bombsPos[i], Quaternion.Euler (bombsRot[i].x, bombsRot[i].y, bombsRot[i].z));
        }

        //LOAD Boxes
        Vector3[] boxesPos = saveObject.boxPositions.ToArray ();
        Vector3[] boxesRot = saveObject.boxAngles.ToArray ();

        for (int i = 0; i < boxesPos.Length; i++) {
            Instantiate (boxPrefab, boxesPos[i], Quaternion.Euler (boxesRot[i].x, boxesRot[i].y, boxesRot[i].z));
        }

        //LOAD Board_0
        Vector3[] board_0Pos = saveObject.board_0Positions.ToArray ();
        Vector3[] board_0Rot = saveObject.board_0Angles.ToArray ();

        for (int i = 0; i < board_0Pos.Length; i++) {
            Instantiate (board_0Prefab, board_0Pos[i], Quaternion.Euler (board_0Rot[i].x, board_0Rot[i].y, board_0Rot[i].z));
        }

        //LOAD Board_30
        Vector3[] board_30Pos = saveObject.board_30Positions.ToArray ();
        Vector3[] board_30Rot = saveObject.board_30Angles.ToArray ();

        for (int i = 0; i < board_30Pos.Length; i++) {
            Instantiate (board_30Prefab, board_30Pos[i], Quaternion.Euler (board_30Rot[i].x, board_30Rot[i].y, board_30Rot[i].z));
        }

        //LOAD Board_m30
        Vector3[] board_m30Pos = saveObject.board_m30Positions.ToArray ();
        Vector3[] board_m30Rot = saveObject.board_m30Angles.ToArray ();

        for (int i = 0; i < board_m30Pos.Length; i++) {
            Instantiate (board_m30Prefab, board_m30Pos[i], Quaternion.Euler (board_m30Rot[i].x, board_m30Rot[i].y, board_m30Rot[i].z));
        }
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
        // Debug.Log("GameLogics " + Time.time.ToString());
        debugManager.Update();
    }

    void LateUpdate()
    {
        debugManager.LateUpdate();
    }


    private class SaveObject {
        public List<Vector3> splinePoints;
        public List<Vector3> coinsPositions;
        public List<Vector3> canisterPositions;
        public List<Vector3> metaPositions;
        public List<Vector3> bombPositions;
        public List<Vector3> bombAngles;

        public List<Vector3> boxPositions;
        public List<Vector3> boxAngles;

        public List<Vector3> board_0Positions;
        public List<Vector3> board_0Angles;

        public List<Vector3> board_30Positions;
        public List<Vector3> board_30Angles;

        public List<Vector3> board_m30Positions;
        public List<Vector3> board_m30Angles;
        public Dictionary<string, List<Vector3>> fantPositions;

    }
}

public class SerializedTransform {
    public float[] _position = new float[3];
    public float[] _rotation = new float[4];
    public float[] _scale = new float[3];

    public SerializedTransform (Transform transform, bool worldSpace = false) {
        _position[0] = transform.localPosition.x;
        _position[1] = transform.localPosition.y;
        _position[2] = transform.localPosition.z;

        _rotation[0] = transform.localRotation.w;
        _rotation[1] = transform.localRotation.x;
        _rotation[2] = transform.localRotation.y;
        _rotation[3] = transform.localRotation.z;

        _scale[0] = transform.localScale.x;
        _scale[1] = transform.localScale.y;
        _scale[2] = transform.localScale.z;

    }
}
public static class SerializedTransformExtention {
    public static void DeserialTransform (this SerializedTransform _serializedTransform, Transform _transform) {
        _transform.localPosition = new Vector3 (_serializedTransform._position[0], _serializedTransform._position[1], _serializedTransform._position[2]);
        _transform.localRotation = new Quaternion (_serializedTransform._rotation[1], _serializedTransform._rotation[2], _serializedTransform._rotation[3], _serializedTransform._rotation[0]);
        _transform.localScale = new Vector3 (_serializedTransform._scale[0], _serializedTransform._scale[1], _serializedTransform._scale[2]);
    }
}

public static class TransformExtention {
    public static void SetTransformEX (this Transform original, Transform copy) {
        original.position = copy.position;
        original.rotation = copy.rotation;
        original.localScale = copy.localScale;
    }
}