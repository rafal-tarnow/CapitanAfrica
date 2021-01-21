using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// public interface IParentFromChildsCollisions
// {
//     void OnTriggerEnter2D(Collider2D other);
//     void OnTriggerStay(Collider other);
//     void OnTriggerExit2D(Collider2D other);
//     void OnCollisionEnter2D(Collision2D other);
//     void OnCollisionStay2D(Collision2D other);
//     void OnCollisionExit2D(Collision2D other);
// }


public class ParentCollisionDetection : MonoBehaviour
{
    //public IParentFromChildsCollisions parent;
    public CarController parent;

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Child: OnTriggerEnter2D");
        parent.OnTriggerEnter2D(other); 
    }

    private void OnTriggerStay(Collider other) {
        //Debug.Log("Child: OnTriggerStay");
        parent.OnTriggerStay(other); 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Child: OnTriggerExit2D");
        parent.OnTriggerExit2D(other);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("Child: OnCollisionEnter2D");
        parent.OnCollisionEnter2D(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("Child: OnCollisionStay2D");
        parent.OnCollisionStay2D(other);
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("Child: OnCollisionExit2D");
        parent.OnCollisionExit2D(other);
    }
}
