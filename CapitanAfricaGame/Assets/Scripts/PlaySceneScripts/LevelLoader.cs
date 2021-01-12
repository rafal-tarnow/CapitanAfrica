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

    public LevelLoader(GameObject mcoinPrefab,
     GameObject mcanisterPrefab,
      GameObject mmetaPrefab, 
      GameObject mbombPrefab, 
      GameObject mboxPrefab, 
      GameObject mboard_0Prefab, 
      GameObject mboard_30Prefab, 
      GameObject mboard_m30Prefab)
    {
        coinPrefab = mcoinPrefab;
        canisterPrefab = mcanisterPrefab;
        metaPrefab = mmetaPrefab;
        bombPrefab = mbombPrefab;
        boxPrefab = mboxPrefab;
        board_0Prefab = mboard_0Prefab;
        board_30Prefab = mboard_30Prefab;
        board_m30Prefab = mboard_m30Prefab;
    }


}

