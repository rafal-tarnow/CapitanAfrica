using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    private static CarController carController;

        void Awake() 
        {
            if (carController == null)
                carController = GameObject.FindWithTag("CarController").GetComponent<CarController>();;
        
        }


    private void OnTriggerEnter2D(Collider2D other) {
        carController.fuel = 1.0f;
        Destroy(gameObject);
    }
}
