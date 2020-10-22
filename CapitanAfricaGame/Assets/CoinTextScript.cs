using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;
    int coinAmount;
    

    private void Awake() {
         text = GetComponent<Text>();
    }

    void Start()
    {
       
    }

    public void setCoins(int coins)
    {
        coinAmount = coins;
        text.text = coinAmount.ToString();
    }
    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
