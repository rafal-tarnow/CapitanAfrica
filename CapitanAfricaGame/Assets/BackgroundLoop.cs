using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public GameObject[] layers;
    private Camera mainCamera;
    private Vector2 screenBounds;
    public float choke;
    public float scrollSpeed;

    void Start(){
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        foreach(GameObject obj in layers){
            loadChildObjects(obj);
        }
    }
    void loadChildObjects(GameObject layer){
        float objectWidth = layer.GetComponent<SpriteRenderer>().bounds.size.x - choke;
        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);
        GameObject clone = Instantiate(layer) as GameObject;
        for(int i = 0; i <= childsNeeded; i++){
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(layer.transform);
            c.transform.position = new Vector3(objectWidth * i, layer.transform.position.y, layer.transform.position.z);
            c.name = layer.name + i;
        }
        Destroy(clone);
        Destroy(layer.GetComponent<SpriteRenderer>());
    }
    void repositionChildObjects(GameObject obj){
         Transform[] children = obj.GetComponentsInChildren<Transform>();
        if(children.Length > 1){
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - choke;
            if(transform.position.x + screenBounds.x > lastChild.transform.position.x + halfObjectWidth){
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObjectWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }else if(transform.position.x - screenBounds.x < firstChild.transform.position.x - halfObjectWidth){
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObjectWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
            }
        }
    }
    void Update() {

        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(scrollSpeed, 0, 0);
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        transform.position = smoothPosition;

    }
    void LateUpdate(){
        foreach(GameObject obj in layers){
            repositionChildObjects(obj);
            obj.transform.position = new Vector3(obj.transform.position.x, mainCamera.transform.position.y, obj.transform.position.z);
        }
    }
}
