using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{ 
    static private  GameManager gameManager;
    //private AudioSource audio;
   
    private void Awake() 
    {
         if (gameManager == null)
            gameManager = GameObject.FindWithTag ("GameLogics").GetComponent<GameManager>();
    }

    private void Start() {
        //if(audio == null)
        //    audio = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {  
        //audio.Play();
        gameManager.OnCoinTriggerEnter2D(this.gameObject, other);
    }
}
