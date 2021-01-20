using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCollisionDetection : MonoBehaviour
{
    public CarController carController;

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Car controller: OnTriggerEnter2D");
        carController.OnTriggerEnter2D(other); 
    }

    private void OnTriggerStay(Collider other) {
        //Debug.Log("Car controller: OnTriggerStay");
        carController.OnTriggerStay(other); 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Car controller: OnTriggerExit2D");
        carController.OnTriggerExit2D(other);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("Car controller: OnCollisionEnter2D");
        carController.OnCollisionEnter2D(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("Car controller: OnCollisionStay2D");
        carController.OnCollisionStay2D(other);
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("Car controller: OnCollisionExit2D");
        carController.OnCollisionExit2D(other);
    }
}
