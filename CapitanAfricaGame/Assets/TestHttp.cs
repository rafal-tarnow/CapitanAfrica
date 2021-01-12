using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestHttp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTexture());
    }


    IEnumerator GetTexture() {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.pngkit.com/png/full/4-43325_transparent-gavel-png-hammer-png.png");
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            Debug.Log("WWW Error !!!");
        }
        else {
            Debug.Log("WWW Success !!!");
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            SpriteRenderer srRend = gameObject.GetComponent<SpriteRenderer> ();
            srRend.sprite = Sprite.Create (myTexture, srRend.sprite.rect, new Vector2 (0, 0), 100);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
