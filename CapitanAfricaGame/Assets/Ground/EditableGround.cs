using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class EditableGround : MonoBehaviour
{
    private SpriteShapeController spriteShapeController;
    Vector3 lastPosition;
    float minimumDistance = 0.3f;
    Spline spline;

   private GameObject redDot;
   List<GameObject> redDotList = new List<GameObject>();
     private int index = 0;
    void Awake() {
        if(redDot == null)
            redDot = GameObject.FindWithTag("RedDot");
  }

    void Start()
    {
        spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;

         for(int i = 0; i < spline.GetPointCount(); i++)
        {
            addRedDot(spline.GetPosition(i));
        }
    }


    void OnDisable()
    {
        foreach (var redDot in redDotList) 
        {
            redDot.SetActive(false);
        }
    }

    void OnEnable()
    {
        redDot.SetActive(true);
        foreach (var redDot in redDotList) 
        {
            redDot.SetActive(true);
        }

    }
    // Start is called before the first frame update

    private void addSplinePoint(Vector3 position)
    {
            spline.InsertPointAt(spline.GetPointCount(), position);
            var newPointIndex = spline.GetPointCount() - 1;
            spline.SetTangentMode(newPointIndex, ShapeTangentMode.Continuous);
            spline.SetHeight(newPointIndex, 1.0f);
            spriteShapeController.BakeCollider();
    }

    public void deleteLastSplinePoint()
    {
            spline.RemovePointAt(spline.GetPointCount() - 1);
    }
    private void addRedDot(Vector3 position)
    {
            if(redDot.activeSelf == false)
                redDot.SetActive(true);
             GameObject cloneRedDot = Instantiate(redDot) as GameObject;
             cloneRedDot.transform.position = position;
             redDotList.Add(cloneRedDot);
    }

    // Update is called once per frame
    void Update()
    {
        var insertPoint = Input.mousePosition;
        insertPoint.z = 0.0f;
        insertPoint = Camera.main.ScreenToWorldPoint(insertPoint);
        insertPoint.z = 0.0f;  
        var m = Mathf.Abs((insertPoint - lastPosition).magnitude);
        if (Input.GetMouseButton(0) && m > minimumDistance)
        { 
            addSplinePoint(insertPoint);
            addRedDot(insertPoint);
            lastPosition = insertPoint;
        }


        // if(index == spline.GetPointCount())
        // {
        //     index = 0;
        // } 
        // redDot.transform.position = spline.GetPosition(index);
        // index++;
            //Debug.Log("lastPosition = " + lastPosition.ToString());
            


    }
}
