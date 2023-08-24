using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndExplosion : MonoBehaviour
{
    [SerializeField]
    public AudioClip myAudioClip;


    void Start()
    {
        AudioSource.PlayClipAtPoint(myAudioClip, Camera.main.transform.position, 1.0f);
        Invoke("DelayedDestory", 0.35f);
    } 

   
    void Update()
    {
        
    }

    void DelayedDestory()
    {
        Destroy(gameObject);
    }
}
