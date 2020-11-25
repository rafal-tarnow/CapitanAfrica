using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomScene : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake() {
        //Application.targetFrameRate = 200;    
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onQuitButton()
    {
        Application.Quit();
    }

    public void onPlayButton()
    {
        SceneManager.LoadScene("LevelSelectScene",LoadSceneMode.Single);
    }

    public void onSettingsButton()
    {

    }
}
