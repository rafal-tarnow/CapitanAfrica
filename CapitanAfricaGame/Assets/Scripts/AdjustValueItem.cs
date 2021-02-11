using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdjustValueItem : MonoBehaviour
{

    public enum Type
    {
        FLOAT,
        BOOLEAN
    }
    public GameLogics.AdjustValue adjustValue;
    public Type type = Type.FLOAT;
    public TMP_Text button_one_txt;
    public TMP_Text button_two_txt;
    private static GameLogics gameLogics;


    TMP_Text label;
    // Start is called before the first frame update

    private void Awake()
    {
        if (gameLogics == null)
            gameLogics = GameObject.FindWithTag("GameLogics").GetComponent<GameLogics>();
    }

    void Start()
    {
        label = transform.GetChild(0).GetComponent<TMP_Text>();
        if (type == Type.FLOAT)
        {
            button_one_txt.text = "+";
            button_two_txt.text = "-";
        }
        else if (type == Type.BOOLEAN)
        {
            button_one_txt.text = "T";
            button_two_txt.text = "F";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == Type.FLOAT)
        {
            label.SetText(adjustValue.ToString() + ": " + gameLogics.getFloatAdjustValue(adjustValue).ToString());
        }
        else if (type == Type.BOOLEAN)
        {
            label.SetText(adjustValue.ToString() + ": " + gameLogics.getBoolAdjustValue(adjustValue).ToString());
        }
    }

    public void onButtonSetPressed()
    {
        Debug.Log("onButtonSetPressed " + adjustValue.ToString());
    }

    public void onButtonOnePressed()
    {
        Debug.Log("onButtonPlusPressed " + adjustValue.ToString());

        if (type == Type.FLOAT)
        {
            float value = gameLogics.getFloatAdjustValue(adjustValue);
            gameLogics.setAdjustValue(adjustValue, value * 1.1f);
        }
        else if (type == Type.BOOLEAN)
        {
            gameLogics.setAdjustValue(adjustValue, true);
        }
    }

    public void onButtonTwoPressed()
    {
        Debug.Log("onButtonMinusPressed " + adjustValue.ToString());


        if (type == Type.FLOAT)
        {
            float value = gameLogics.getFloatAdjustValue(adjustValue);
            gameLogics.setAdjustValue(adjustValue, value / 1.1f);
        }
        else if (type == Type.BOOLEAN)
        {
            gameLogics.setAdjustValue(adjustValue, false);
        }
    }
}
