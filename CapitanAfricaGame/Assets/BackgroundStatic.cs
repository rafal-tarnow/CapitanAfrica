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

     float antyOverlap = 1.0f;

    void Start(){
        offset = transform.position - follow_target.position;  //Camera follow


        Camera cam = gameObject.GetComponent<Camera>();
        cameraHeight = 2f * cam.orthographicSize;
        cameraWidth = cameraHeight * cam.aspect;

        imageHeight = image.GetComponent<SpriteRenderer>().bounds.size.y;
        float heightScale = cameraHeight/imageHeight;
        image.transform.localScale = new Vector3(heightScale, heightScale, 1.0f);

        imageWidthAfterScale = image.GetComponent<SpriteRenderer>().bounds.size.x;
        childsNeeded = (int)Mathf.Ceil(cameraWidth / imageWidthAfterScale);



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


    Vector3 futureCamPosition = new Vector3();

    void Update(){
        // if (Physics2D.simulationMode == SimulationMode2D.Script)
        // {
        //     Physics2D.Simulate(1.0f/60.0f);
        // }

   
        
    }

    private void LateUpdate() {
        transform.position = follow_target.position + offset;    //camera follow


             futureCamPosition = follow_target.position + offset;
        run = (futureCamPosition.x/40.0f);

        float howMany = Mathf.Floor(run/imageWidthAfterScale);

        run = run - howMany*imageWidthAfterScale;
        image.transform.position = new Vector3(futureCamPosition.x - run, futureCamPosition.y, image.transform.position.z);
    }

}
