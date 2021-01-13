using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandleButtons : MonoBehaviour
{
    public TMP_Text readTextOutput;
    public TMP_InputField setTextInput;

    string setURL = "localhost:80/tutorial_1/PostName.php?name=";
    string getURL = "localhost:80/tutorial_1/ReadName.php";

    public void SetText()
    {
        StartCoroutine(SetTheText(setTextInput.text));
    }

    IEnumerator SetTheText(string name) 
    {
        string URL = setURL + name;
        WWW www = new WWW(URL);
        yield return www;
    }

    public void GetText(){
        StartCoroutine(GetTheText());
    }

    IEnumerator GetTheText()
    {
        string URL = getURL;
        WWW www = new WWW(URL);
        yield return www;
        readTextOutput.text = www.text;       
    }
}
