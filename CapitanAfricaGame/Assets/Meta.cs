using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    private static GameLogics gameLogics;
    public CarController carController;

    private void Awake()
    {
        if(gameLogics == null)
            gameLogics = GameObject.FindWithTag("GameLogics").GetComponent<GameLogics>();

    }
    private void OnTriggerEnter2D(Collider2D other) {
         gameLogics.onMetaReached();
    }
}
