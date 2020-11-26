using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdjustValueItem : MonoBehaviour
{

    private static GameLogics gameLogics;          

    public GameLogics.AdjustValue adjustValue;
    TMP_Text label;
    // Start is called before the first frame update

    private void Awake() {
        if(gameLogics == null)
            gameLogics = GameObject.FindWithTag ("GameLogics").GetComponent<GameLogics> ();
    }

    void Start()
    {
        label = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        label.SetText(adjustValue.ToString() + ": " + gameLogics.getAdjustValue(adjustValue).ToString());
    }

    public void onButtonSetPressed()
    {
        Debug.Log("onButtonSetPressed " + adjustValue.ToString());
    }

    public void onButtonPlusPressed()
    {
        Debug.Log("onButtonPlusPressed " + adjustValue.ToString());
        float value = gameLogics.getAdjustValue(adjustValue);
        gameLogics.setAdjustValue(adjustValue, value*1.1f);
    }

    public void onButtonMinusPressed()
    {
        Debug.Log("onButtonMinusPressed " + adjustValue.ToString());
        float value = gameLogics.getAdjustValue(adjustValue);
        gameLogics.setAdjustValue(adjustValue, value/1.1f);
    }
}
