using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{ 
    static private  GameLogics gameLogics;
    //private AudioSource audio;
   
    private void Awake() 
    {
         if (gameLogics == null)
            gameLogics = GameObject.FindWithTag ("GameLogics").GetComponent<GameLogics>();
    }

    private void Start() {
        //if(audio == null)
        //    audio = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {  
        //audio.Play();
        gameLogics.OnCoinTriggerEnter2D(this.gameObject, other);
    }
}
