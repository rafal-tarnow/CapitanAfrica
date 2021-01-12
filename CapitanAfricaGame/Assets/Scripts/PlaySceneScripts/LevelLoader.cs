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

        
        
        SaveObject saveObject = SaveSystem.Load<SaveObject> (levelFileName);

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

}

