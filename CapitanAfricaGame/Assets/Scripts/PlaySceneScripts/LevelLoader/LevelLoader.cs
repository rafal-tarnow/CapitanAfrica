using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{

    private GameObject coinPrefab;
    private GameObject canisterPrefab;
    private GameObject metaPrefab;
    private GameObject bombPrefab;
    private GameObject boxPrefab;
    private GameObject board_0Prefab;
    private GameObject board_30Prefab;
    private GameObject board_m30Prefab;

    private GameObject groundEditable;

    public LevelLoader(GameObject mcoinPrefab,
     GameObject mcanisterPrefab,
      GameObject mmetaPrefab, 
      GameObject mbombPrefab, 
      GameObject mboxPrefab, 
      GameObject mboard_0Prefab, 
      GameObject mboard_30Prefab, 
      GameObject mboard_m30Prefab,
      GameObject mgroundEditable)
    {
        coinPrefab = mcoinPrefab;
        canisterPrefab = mcanisterPrefab;
        metaPrefab = mmetaPrefab;
        bombPrefab = mbombPrefab;
        boxPrefab = mboxPrefab;
        board_0Prefab = mboard_0Prefab;
        board_30Prefab = mboard_30Prefab;
        board_m30Prefab = mboard_m30Prefab;
        groundEditable = mgroundEditable;

        SaveSystem.Init();
    }

    public void LoadLevel(string levelFileName)
    {
        DestroyAllObjects ("Coin");
        DestroyAllObjects ("FuelCanister");
        DestroyAllObjects ("Meta");
        DestroyAllObjects ("Box");
        DestroyAllObjects ("Bomb");
        DestroyAllObjects ("Board_0");
        DestroyAllObjects ("Board_30");
        DestroyAllObjects ("Board_m30");

        
        
        SaveObject saveObject = SaveSystem.Load<SaveObject> (Paths.LEVELS_EDIT + levelFileName, Paths.TEMPLATES + "template_level.txt");

        //LOAD GROUND
        if (saveObject == null)
            UnityEngine.Debug.Log ("Load retun null");
        else
            groundEditable.GetComponent<EditableGround> ().loadPoints (saveObject.splinePoints);

        //LOAD COINS
        Vector3[] coinsPos = saveObject.coinsPositions.ToArray ();

        for (int i = 0; i < coinsPos.Length; i++) {
            GameObject.Instantiate (coinPrefab, coinsPos[i], Quaternion.identity);
        }

        //LOAD CANISTERS
        Vector3[] canisterPos = saveObject.canisterPositions.ToArray ();

        for (int i = 0; i < canisterPos.Length; i++) {
            GameObject.Instantiate (canisterPrefab, canisterPos[i], Quaternion.identity);
        }

        //LOAD META
        Vector3[] metasPos = saveObject.metaPositions.ToArray ();

        for (int i = 0; i < metasPos.Length; i++) {
            GameObject.Instantiate (metaPrefab, metasPos[i], Quaternion.identity);
        }

        //LOAD Bombs
        Vector3[] bombsPos = saveObject.bombPositions.ToArray ();
        Vector3[] bombsRot = saveObject.bombAngles.ToArray ();

        for (int i = 0; i < bombsPos.Length; i++) {
            GameObject.Instantiate (bombPrefab, bombsPos[i], Quaternion.Euler (bombsRot[i].x, bombsRot[i].y, bombsRot[i].z));
        }

        //LOAD Boxes
        Vector3[] boxesPos = saveObject.boxPositions.ToArray ();
        Vector3[] boxesRot = saveObject.boxAngles.ToArray ();

        for (int i = 0; i < boxesPos.Length; i++) {
            GameObject.Instantiate (boxPrefab, boxesPos[i], Quaternion.Euler (boxesRot[i].x, boxesRot[i].y, boxesRot[i].z));
        }

        //LOAD Board_0
        Vector3[] board_0Pos = saveObject.board_0Positions.ToArray ();
        Vector3[] board_0Rot = saveObject.board_0Angles.ToArray ();

        for (int i = 0; i < board_0Pos.Length; i++) {
            GameObject.Instantiate (board_0Prefab, board_0Pos[i], Quaternion.Euler (board_0Rot[i].x, board_0Rot[i].y, board_0Rot[i].z));
        }

        //LOAD Board_30
        Vector3[] board_30Pos = saveObject.board_30Positions.ToArray ();
        Vector3[] board_30Rot = saveObject.board_30Angles.ToArray ();

        for (int i = 0; i < board_30Pos.Length; i++) {
            GameObject.Instantiate (board_30Prefab, board_30Pos[i], Quaternion.Euler (board_30Rot[i].x, board_30Rot[i].y, board_30Rot[i].z));
        }

        //LOAD Board_m30
        Vector3[] board_m30Pos = saveObject.board_m30Positions.ToArray ();
        Vector3[] board_m30Rot = saveObject.board_m30Angles.ToArray ();

        for (int i = 0; i < board_m30Pos.Length; i++) {
            GameObject.Instantiate (board_m30Prefab, board_m30Pos[i], Quaternion.Euler (board_m30Rot[i].x, board_m30Rot[i].y, board_m30Rot[i].z));
        }
    }

    void DestroyAllObjects (string tag) 
    {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag (tag);

            for (var i = 0; i < gameObjects.Length; i++) 
            {
                GameObject.Destroy (gameObjects[i]);
            }
    }

    public void SaveLevel (string levelFileName) 
    {

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

        SaveSystem.Save<SaveObject> (saveObject, Paths.LEVELS_EDIT + levelFileName);
    }

}


class SaveObject {
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
