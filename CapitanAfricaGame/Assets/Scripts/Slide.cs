using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Slide : MonoBehaviour
{
    int count = 0;
    int prevCount = -1;
    public GameObject Right;
    public GameObject Left;
    public GameObject center;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public GameObject levelLockedPanel;

    public GameObject imageWorld_1;
    public GameObject imageWorld_2;

    public GameObject imageWorld_3;
    public Button leftButton;
    public Button rightButton;
    // Start is called before the first frame update
    public Sprite rightButtonActiveSprite;
    public Sprite rightButtonInactiveSprite;

    public Sprite leftButtonActiveSprite;
    public Sprite leftButtonInactiveSprite;

    public Sprite lockButtonImage;


    void Start()
    {
        var buttons = GameObject.FindGameObjectsWithTag("ButtonLevel");
        int  unlockedLevelIndex = PlayerPrefs.GetInt("unlockedLevelIndex", 0);
        Button button;
        for(int i = 0; i < buttons.Length; i++)
        {
                button = buttons[i].GetComponent<Button>();
                string buttonName = button.name;

                button.onClick.AddListener(() => runLevel(buttonName));
                if(i > unlockedLevelIndex)
                    button.GetComponent<ButtonLock>().setLock(true);
        }

        if(levelLockedPanel ==  null)
            levelLockedPanel = GameObject.Find("PanelLock");

        levelLockedPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
            obj1.transform.position = Vector3.Lerp(obj1.transform.position, center.transform.position, 10f * Time.deltaTime);
            obj2.transform.position = Vector3.Lerp(obj2.transform.position, Right.transform.position, 10f * Time.deltaTime);
            obj3.transform.position = Vector3.Lerp(obj3.transform.position, Right.transform.position, 10f * Time.deltaTime);

            imageWorld_1.transform.position = Vector3.Lerp(imageWorld_1.transform.position, center.transform.position, 1f);
            imageWorld_2.transform.position = Vector3.Lerp(imageWorld_2.transform.position, Right.transform.position, 1f);
            imageWorld_3.transform.position = Vector3.Lerp(imageWorld_3.transform.position, Right.transform.position, 1f);

        }
        else if (count == 1)
        {
            obj1.transform.position = Vector3.Lerp(obj1.transform.position, Left.transform.position, 10f * Time.deltaTime);
            obj2.transform.position = Vector3.Lerp(obj2.transform.position, center.transform.position, 10f * Time.deltaTime);
            obj3.transform.position = Vector3.Lerp(obj3.transform.position, Right.transform.position, 10f * Time.deltaTime);

            imageWorld_1.transform.position = Vector3.Lerp(imageWorld_1.transform.position, Left.transform.position, 10f);
            imageWorld_2.transform.position = Vector3.Lerp(imageWorld_2.transform.position, center.transform.position, 1f);
            imageWorld_3.transform.position = Vector3.Lerp(imageWorld_3.transform.position, Right.transform.position, 1f);
        }
        else if (count == 2)
        {
            obj1.transform.position = Vector3.Lerp(obj1.transform.position, Left.transform.position, 10f * Time.deltaTime);
            obj2.transform.position = Vector3.Lerp(obj2.transform.position, Left.transform.position, 10f * Time.deltaTime);
            obj3.transform.position = Vector3.Lerp(obj3.transform.position, center.transform.position, 10f * Time.deltaTime);

            imageWorld_1.transform.position = Vector3.Lerp(imageWorld_1.transform.position, Left.transform.position, 10f);
            imageWorld_2.transform.position = Vector3.Lerp(imageWorld_2.transform.position, Left.transform.position, 1f);
            imageWorld_3.transform.position = Vector3.Lerp(imageWorld_3.transform.position, center.transform.position, 1f);
        }

        updateButtonsActiv();
    }

    public void Right_Click()
    {
        count++;
        if (count > 2)
            count = 2;

    }

    public void Left_click()
    {
        count--;
        if (count < 0)
            count = 0;
    }

    private void updateButtonsActiv()
    {
        if (prevCount != count)
        {
            prevCount = count;

            if (count == 0)
            {
                leftButton.GetComponent<Image>().sprite = leftButtonInactiveSprite;
            }
            else
            {
                leftButton.GetComponent<Image>().sprite = leftButtonActiveSprite;
            }

            if (count == 2)
            {
                rightButton.GetComponent<Image>().sprite = rightButtonInactiveSprite;
            }
            else
            {
                rightButton.GetComponent<Image>().sprite = rightButtonActiveSprite;
            }
        }

    }


    void runLevel(string buttonName)
    {
        int  unlockedLevelIndex = PlayerPrefs.GetInt("unlockedLevelIndex", 0);
        int level = getLevelIndexFromButtonName(buttonName);

        Debug.Log("unlockedLevelIndex " +  unlockedLevelIndex.ToString());
        Debug.Log("level " +  level.ToString());

        if(level > unlockedLevelIndex)
        {
            levelLockedPanel.SetActive(true);
        }
        else
        {
            ScenesVariablePass.levelToRun = level;
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    private int getLevelIndexFromButtonName(string buttonName)
    {
        string[] words = buttonName.Split('_');
        return int.Parse(words[1]);
    }

    public void onBackButtonPressed()
    {
        SceneManager.LoadScene("WelcomeScene", LoadSceneMode.Single);
    }
}


