using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStatic : MonoBehaviour
{
    public Transform follow_target; //camera follow
    private Vector3 offset; //camera follow

     public GameObject image;
     float cameraHeight;
     float cameraWidth;

     float imageWidthAfterScale;
     float imageHeight;
     int childsNeeded;

     float run = 0.0f;

    void Start(){
  offset = transform.position - follow_target.position;  //Camera follow


        Camera cam = gameObject.GetComponent<Camera>();
        cameraHeight = 2f * cam.orthographicSize;
        cameraWidth = cameraHeight * cam.aspect;

        imageHeight = image.GetComponent<SpriteRenderer>().bounds.size.y;
        float heightScale = cameraHeight/imageHeight;
        image.transform.localScale = new Vector3(heightScale, heightScale, 1.0f);

        imageWidthAfterScale = image.GetComponent<SpriteRenderer>().bounds.size.x;
        childsNeeded = (int)Mathf.Ceil(cameraWidth / imageWidthAfterScale) * 10;



        float start_x = (cameraWidth - imageWidthAfterScale)/2.0f;

        GameObject clone = Instantiate(image) as GameObject;
        for(int i = 0; i <= childsNeeded; i++){
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(image.transform);
            c.transform.position = new Vector3(imageWidthAfterScale * i - start_x, image.transform.position.y, image.transform.position.z);
            c.name = image.name + i;
        }
        Destroy(clone);
        Destroy(image.GetComponent<SpriteRenderer>());
        

    }

    void Update() {

    }
    void LateUpdate(){
        transform.position = follow_target.position + offset;    //camera follow

        run = transform.position.x/40.0f; 
        image.transform.position = new Vector3(transform.position.x - run, transform.position.y, image.transform.position.z);
    }

}
