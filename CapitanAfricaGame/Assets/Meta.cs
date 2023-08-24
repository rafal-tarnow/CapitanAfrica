using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    private static GameManager gameManager;
    public CarController carController;

    private void Awake()
    {
        if(gameManager == null)
            gameManager = GameObject.FindWithTag("GameLogics").GetComponent<GameManager>();

    }
    private void OnTriggerEnter2D(Collider2D other) {
         gameManager.onMetaReached();
    }
}
