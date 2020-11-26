using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTextScriptMP : MonoBehaviour
{
    private TMP_Text text;
    int coinAmount = 0;
    
    private void Awake() 
    {
        if(text == null)
            text = GetComponent<TMP_Text>();
    }

    private void Start() 
    {
        //text.SetText("The first number is {0} and the 2nd is {1:2} and the 3rd is {3:0}.", 4, 6.345f, 3.5f);
        text.SetText("0");
    }

    public void setCoins(int coins)
    {
        if(coinAmount != coins)
        {    
            coinAmount = coins;
            text.SetText(coinAmount.ToString());
        }
    }
}
