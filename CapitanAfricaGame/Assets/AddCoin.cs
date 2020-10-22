using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{
    
    static private  GameLogics gameLogics;

    private void Awake() {
         if (gameLogics == null)
            gameLogics = GameObject.FindWithTag ("GameLogics").GetComponent<GameLogics>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        gameLogics.OnCoinTriggerEnter2D(this.gameObject, other);
    }
}
