﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{
    public CarController carController;
    private void OnTriggerEnter2D(Collider2D other) {
        CoinTextScript.coinAmount += 1;
        Destroy(gameObject);
    }
}