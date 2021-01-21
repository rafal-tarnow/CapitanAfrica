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
    private GameObject canvasMeta;

    private AudioSource coinAudioSource;
    private AudioSource musicAudioSource;

    private CoinTextScriptMP coinTextScriptMP;

    float cameraStartupOrthographicSize;

    AdjustManager adjustManager = new AdjustManager();
    DebugManager debugManager = new DebugManager();
    LevelLoader levelLoader;

    void Awake () 
    {
        Application.targetFrameRate = 60;
        //UnityEngine.Debug.unityLogger.logEnabled = false;


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

        if(canvasMeta == null)
            canvasMeta = GameObject.FindWithTag("CanvasMeta");

        if(canvasEdit == null)
            canvasEdit = GameObject.FindWithTag("CanvasEdit");

        if((coinAudioSource == null) || (musicAudioSource == null))
        {   
            var audios = this.GetComponents<AudioSource>();
            coinAudioSource = audios[0];
            musicAudioSource = audios[1];
        }

    }

    private void Start () 
    {
        cameraStartupOrthographicSize = Camera.main.orthographicSize;

        mode = MainMode.PLAY;
        editSubMode = EditSubMode.NONE;

        if(levelLoader == null)
            levelLoader = new LevelLoader(coinPrefab, canisterPrefab, metaPrefab, bombPrefab, boxPrefab, board_0Prefab, board_30Prefab, board_m30Prefab, groundEditable);

        LoadLevel ();

        updateUIState (true);

        coinAmount = PlayerPrefs.GetInt("Coins",0);
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

    public void restartCurrentScene(bool pressed) {
        Physics2D.simulationMode = SimulationMode2D.Update;

        if (!pressed) // if released
            SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }

    public void runNextLevel()
    {
        Physics2D.simulationMode = SimulationMode2D.Update;
        #warning 'Check is next level exist to run'
        ScenesVariablePass.levelToRun++;
        SceneManager.LoadScene (SceneManager.GetActiveScene().name);
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


    public void OnUIInventoryDragEvent (bool isDragging) 
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

    public void onEditButton () 
    {
        mode = MainMode.EDIT;
        LoadLevel ();
        updateUIState (true);
    }


    public void onPlayButton () 
    {
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


    public void onGasButton(bool state)
    {
        carControllerScript.onGasPedalPressedEvent(state);
    }


    public void onBrakeButton(bool state)
    {
        carControllerScript.onBrakePedalPressedEvent(state); 
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

    public void onMetaReached () 
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

        if(unlockedLevel < ScenesVariablePass.levelToRun + 1)
        {
            PlayerPrefs.SetInt("unlockedLevelIndex", ScenesVariablePass.levelToRun + 1);
            PlayerPrefs.Save();
        }
    }


    private void updateUIState (bool alignInventory) 
    {
        if (mode == MainMode.PLAY) 
        {
            musicAudioSource.Play();

            backgroundSprite.SetActive (true);
            Camera.main.GetComponent<BackgroundStatic> ().enabled = true;
            Camera.main.GetComponent<PanZoom> ().enabled = false;
            Camera.main.orthographicSize = cameraStartupOrthographicSize;

            groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.PLAY);

            carController.SetActive (true);
            // carController.GetComponent<FollowByCamera>().enabled = false;
            // carController.GetComponent<CarController>().enabled = true;

            //CANVAS
            canvasEdit.SetActive(false);
            canvasPlay.SetActive(true);
            canvasAdjust.SetActive(false);
            canvasMeta.SetActive(false);

            
            setAllFants_Opacity_Dragable_Deletable (1.0f, false, false);

            Physics2D.simulationMode = SimulationMode2D.Update;

        } 
        else if (mode == MainMode.EDIT) 
        {
            musicAudioSource.Pause();

            //carController.GetComponent<FollowByCamera>().enabled = true;
            //carController.GetComponent<CarController>().enabled = false;
            //carController.GetComponent<Rigidbody2D>().isKinematic = true;
            carController.transform.eulerAngles = new Vector3 (0, 0, 0);
            carController.SetActive (false);

            Camera.main.GetComponent<BackgroundStatic> ().enabled = false;
            Camera.main.GetComponent<PanZoom> ().enabled = true;

            backgroundSprite.SetActive (false);

            canvasPlay.SetActive(false);
            canvasEdit.SetActive(true);
            canvasAdjust.SetActive(false);
            canvasMeta.SetActive(false);

            if (editSubMode == EditSubMode.ADD_FANT) 
            {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_INACTIVE);

                inventoryUI.SetActive (true);
                if(alignInventory)
                    inventoryUI.GetComponent<UnityEngine.UI.ScrollRect> ().horizontalNormalizedPosition = 1.0f;
                
                setAllFants_Opacity_Dragable_Deletable (1.0f, true, false);
            } 
            else if (editSubMode == EditSubMode.EDIT_GROUND) 
            {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_DRAG_ADD_POINTS);
                
                inventoryUI.SetActive (false);

                setAllFants_Opacity_Dragable_Deletable (0.3f, false, false);
            } 
            else if (editSubMode == EditSubMode.DELETE_FANT) 
            {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (true);

                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_DELETE_POINTS);
                
                inventoryUI.SetActive (false);

                setAllFants_Opacity_Dragable_Deletable (1.0f, false, true);
            } 
            else if (editSubMode == EditSubMode.NONE) 
            {
                buttonDrag.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonEditGround.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
                buttonTrash.GetComponent<ButtonBistable> ().SetStateWithoutEvent (false);
               
                groundEditable.GetComponent<EditableGround>().setMode(EditableGround.Mode.EDIT_INACTIVE);

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

    public void SaveLevel () 
    {
        levelLoader.SaveLevel(getCurrentLevelFileName());
    }

    private void loadFantsPositionsToSaveObjectByTag (ref SaveObject saveObject, string tag) {
        GameObject[] fants;
        fants = GameObject.FindGameObjectsWithTag (tag);
        saveObject.fantPositions[tag] = new List<Vector3> ();

        foreach (var fant in fants) {
            saveObject.fantPositions[tag].Add (fant.transform.position);
        }
    }


    string getCurrentLevelFileName()
    {
        int levelIndex = ScenesVariablePass.levelToRun;
        string levelFileName = levelIndex.ToString() + ".txt";
        return levelFileName;
    }


    public void LoadLevel () 
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


    