using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFuel : MonoBehaviour
{
    public CarController carController;
    private void OnTriggerEnter2D(Collider2D other) {
        carController.fuel = 1.0f;
        Destroy(gameObject);
    }
}
