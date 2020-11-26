using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextScript : MonoBehaviour
{
    Text text;
    int coinAmount = 0;
    
    private void Awake() 
    {
        if(text == null)
            text = GetComponent<Text>();
    }

    private void Start() 
    {
        text.text = coinAmount.ToString();
    }

    public void setCoins(int coins)
    {
        if(coinAmount != coins)
        {    
            coinAmount = coins;
            text.text = coinAmount.ToString();
        }
    }
}
