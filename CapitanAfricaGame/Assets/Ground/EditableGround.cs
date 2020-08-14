using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;
using UnityEngine.EventSystems;

public class EditableGround : MonoBehaviour
{
    private SpriteShapeController spriteShapeController;
    Vector3 lastPosition;
    float minimumDistance = 0.3f;
    Spline spline;
    bool blockNewPoints = false;
   private GameObject redDot;

   List<GameObject> redDotList = new List<GameObject>();
   Dictionary<GameObject, int> redDotMap = new Dictionary<GameObject, int>();
     private int index = 0;
    void Awake() {
        if(redDot == null)
        {
            redDot = GameObject.FindWithTag("RedDot");
            redDot.SetActive(false);
        }

        spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;

  }

    void Start()
    {

    }

    public List<Vector3> getSplinesPointsPositions()
    {
        List<Vector3> points = new List<Vector3>();
        for(Int32 i = 0; i < spline.GetPointCount(); i++)
        {
            points.Add(spline.GetPosition(i));
        }
        return points;
    }
    void OnDisable()
    {

        spriteShapeController.BakeCollider(); 
        clearAllRedDots();

    }

    private void clearAllRedDots()
    {
        redDotMap.Clear();
        foreach(var obj in redDotList)
        {
            Destroy(obj);
        }
        redDotList.Clear();
    }

    void OnEnable()
    {
        for(int i = 0; i < spline.GetPointCount(); i++)
        {
            addRedDot(spline.GetPosition(i), i);
        }
    }
    // Start is called before the first frame update

    private int addSplinePoint(Vector3 position)
    {
            spline.InsertPointAt(spline.GetPointCount(), position);
            var newPointIndex = spline.GetPointCount() - 1;
            spline.SetTangentMode(newPointIndex, ShapeTangentMode.Continuous);
            spline.SetHeight(newPointIndex, 1.0f);
            return newPointIndex;
    }

    private void changeSplinePointPosition(Int32 index, Vector3 position)
    {
        spline.SetPosition(index, position);
    }

    public void onRedDotPositionChanged(GameObject gameObject)
    {
        //Debug.Log("Red dot pos changed = " + position.ToString());
        Int32 index = redDotMap[gameObject];
        Vector3 position = gameObject.transform.localPosition;

        changeSplinePointPosition(index, position);
    }
    private void addRedDot(Vector3 position, int splineIndex)
    {
            if(redDot.activeSelf == false)
            redDot.SetActive(true);
             GameObject cloneRedDot = Instantiate(redDot) as GameObject;
             redDot.SetActive(false);
             cloneRedDot.transform.position = position;
             cloneRedDot.GetComponent<DragableSprite>().OnPositionChanged.AddListener(onRedDotPositionChanged);
             redDotList.Add(cloneRedDot);
             redDotMap.Add(cloneRedDot, splineIndex);
    }

    public void blockAddNewPoints(bool block)
    {
        blockNewPoints = block;
    }
    // Update is called once per frame
private bool IsPointerOverUIObject() 
{ 
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current); 
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 
        List<RaycastResult> results = new List<RaycastResult>(); 
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results); 
        return results.Count > 0; 
}


    public void loadPoints(List<Vector3> positions)
    {
        clearAllRedDots();

        spline.Clear();
        foreach(var position in positions)
        {
            int splineIndex = addSplinePoint(position);
            addRedDot(position, splineIndex);
        }                
        
    }
    void Update()
    {
        if(IsPointerOverUIObject())
            return;
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // check touch over ui
        // {
        //     // Check if finger is over a UI element
        //     //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //     if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //     {
        //         return;
        //     }
        // }

        // if (Input.GetMouseButtonDown(0)) // check mouse click over ui
        // {
        //     // Check if the mouse was clicked over a UI element
        //     if (EventSystem.current.IsPointerOverGameObject())
        //     {
        //         return;
        //     }
        // }


        if(!blockNewPoints)
        {
            var insertPoint = Input.mousePosition;
            insertPoint.z = 0.0f;
            insertPoint = Camera.main.ScreenToWorldPoint(insertPoint);
            insertPoint.z = 0.0f;  
            var m = Mathf.Abs((insertPoint - lastPosition).magnitude);
            if (Input.GetMouseButton(0) && m > minimumDistance)
            { 
                int splineIndex = addSplinePoint(insertPoint);
                addRedDot(insertPoint, splineIndex);
                lastPosition = insertPoint;
            }

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
