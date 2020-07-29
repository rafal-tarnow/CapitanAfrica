using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        CoinTextScript.coinAmount += 1;
        Destroy(gameObject);
    }
}
