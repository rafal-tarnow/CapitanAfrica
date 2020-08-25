using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EditableGround : MonoBehaviour
{
    public UnityEvent<bool> OnDragRedDotEvent;
    private SpriteShapeController spriteShapeController;
    Vector3 lastPosition;
    float minimumDistance = 0.3f;
    Spline spline;
    bool isEditingEnable = false;
    bool dragRedDotMode = false;
    private GameObject redDot;
    private Vector3 touchStartPos;

    List<GameObject> redDotList = new List<GameObject>();
    Dictionary<GameObject, int> redDotMap = new Dictionary<GameObject, int>();
    void Awake()
    {
        if (redDot == null)
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
        for (Int32 i = 0; i < spline.GetPointCount(); i++)
        {
            points.Add(spline.GetPosition(i));
        }
        return points;
    }
    void OnDisable()
    {
        spriteShapeController.BakeCollider();
        destroyAllRedDots();
    }

    void OnEnable()
    {
        updateAllRedDots();
    }
    private void destroyAllRedDots()
    {
        redDotMap.Clear();
        foreach (var obj in redDotList)
        {
            Destroy(obj);
        }
        redDotList.Clear();
    }
    private void activateAllRedDots(bool activate)
    {
        foreach (var obj in redDotList)
        {
            obj.SetActive(activate);
        }
    }

    private void updateAllRedDots()
    {
        destroyAllRedDots();

        //MAKE CLONE
        if (redDot.activeSelf == false)
            redDot.SetActive(true);


        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            GameObject cloneRedDot = Instantiate(redDot) as GameObject;
            cloneRedDot.transform.position = spline.GetPosition(i);
            cloneRedDot.GetComponent<DragableSprite>().OnBeginDrag.AddListener(onRedDotBeginDrag);
            cloneRedDot.GetComponent<DragableSprite>().OnPositionChanged.AddListener(onRedDotDrag);
            cloneRedDot.GetComponent<DragableSprite>().OnEndDrag.AddListener(onRedDotEndDrag);
            redDotList.Add(cloneRedDot);
            redDotMap.Add(cloneRedDot, i);
        }

        redDot.SetActive(false);
    }


    // Start is called before the first frame update

    private void Smoothen(SpriteShapeController sc, int pointIndex)
    {
        Vector3 position = sc.spline.GetPosition(pointIndex);
        Vector3 positionNext = sc.spline.GetPosition(SplineUtility.NextIndex(pointIndex, sc.spline.GetPointCount()));
        Vector3 positionPrev = sc.spline.GetPosition(SplineUtility.PreviousIndex(pointIndex, sc.spline.GetPointCount()));
        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;

        sc.spline.SetTangentMode(pointIndex, ShapeTangentMode.Continuous);
        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);

        sc.spline.SetLeftTangent(pointIndex, leftTangent);
        sc.spline.SetRightTangent(pointIndex, rightTangent);
    }

    private int addSplinePoint2_atEnd(Vector3 position)
    {
        spline.InsertPointAt(spline.GetPointCount(), position);
        var newPointIndex = spline.GetPointCount() - 1;
        Smoothen(spriteShapeController, newPointIndex - 1);
        spline.SetHeight(newPointIndex, 1.0f);
        return newPointIndex;
    }

    private int insertSplinePoint(Vector3 position)
    {
        float minDistance = float.MaxValue;
        int minDistanceIndex = 0;

        if (spline.GetPointCount() < 2)
            addSplinePoint2_atEnd(position);

        if (spline.isOpenEnded)
        {
            int pointCount = spline.GetPointCount();
            for (int i = 0; i < pointCount - 1; i++)
            {
                float distance = Calc.DistancePointLine(position, spline.GetPosition(i), spline.GetPosition(i + 1));
                Debug.Log("distance = " + distance);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceIndex = i;
                }
            }
            minDistanceIndex++;
        }
        else
        {
            float distance = float.MaxValue;
            int pointCount = spline.GetPointCount();

            for (int i = 0; i < pointCount - 1; i++)
            {
                distance = Calc.DistancePointLine(position, spline.GetPosition(i), spline.GetPosition(i + 1));
 //               Debug.Log("distance = " + distance);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceIndex = i;
                }
            }
            minDistanceIndex++;
            //additionaly measure distance beetwen last and first point because it is closed
            distance = Calc.DistancePointLine(position, spline.GetPosition(0), spline.GetPosition(pointCount - 1));
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceIndex = pointCount;
            }

        }


//        Debug.Log("minDistance = " + minDistance);
//        Debug.Log("minDistanceIndex = " + minDistanceIndex);

        spline.InsertPointAt(minDistanceIndex, position);
        Smoothen(spriteShapeController, minDistanceIndex - 1);
        spline.SetHeight(minDistanceIndex, 1.0f);
        return minDistanceIndex;
    }
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
        Smoothen(spriteShapeController, index - 1);

    }


    public void onRedDotBeginDrag(GameObject gameObject)
    {
              OnDragRedDotEvent?.Invoke(true);
        dragRedDotMode = true;
    }

    public void onRedDotEndDrag(GameObject gameObject)
    {
        dragRedDotMode = false;
        OnDragRedDotEvent?.Invoke(false);
    }
    public void onRedDotDrag(GameObject gameObject)
    {
        //Debug.Log("Red dot pos changed = " + position.ToString());
        Int32 index = redDotMap[gameObject];
        Vector3 position = gameObject.transform.localPosition;

        changeSplinePointPosition(index, position);
    }

    public void enableEditing(bool enableEditing)
    {
        isEditingEnable = enableEditing;
        activateAllRedDots(enableEditing);
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
        spline.Clear();

        foreach (var position in positions)
        {
            int splineIndex = addSplinePoint(position);
            if (splineIndex > 3)
                Smoothen(spriteShapeController, splineIndex - 1);
        }
        updateAllRedDots();
        activateAllRedDots(isEditingEnable);
    }

    private float distanceInMilimeters(Vector3 p1, Vector3 p2)
    {
        float distanceInPixels = Vector3.Distance(p1,p2);
        Debug.Log("distanceInPixels" + distanceInPixels);

        float dpi = Screen.dpi;
        Debug.Log("dpi = " + dpi);
        if(dpi < 1)
            dpi = 424.0f;
        #warning Make default dpi dependend on platform


        float distanceInInches  = distanceInPixels/dpi;
        Debug.Log("distance in inches" + distanceInInches);
        float distInMilimeters = distanceInInches * 25.4f;
        return distInMilimeters;
    }
    void Update()
    {
        if (!isEditingEnable)
        {
            return;
        }

        if (IsPointerOverUIObject())
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



        // var insertPoint = Input.mousePosition;
        // insertPoint.z = 0.0f;
        // insertPoint = Camera.main.ScreenToWorldPoint(insertPoint);
        // insertPoint.z = 0.0f;  
        // var m = Mathf.Abs((insertPoint - lastPosition).magnitude);
        // if (Input.GetMouseButton(0) && m > minimumDistance)
        // { 
        //     int splineIndex = addSplinePoint2(insertPoint);

        //     addRedDot(insertPoint, splineIndex);
        //     lastPosition = insertPoint;
        // }


        if(dragRedDotMode)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
                var mp = Input.mousePosition;
                mp.z = 0.0f;

                touchStartPos.z = 0.0f;

                float distInMilimeters = distanceInMilimeters(touchStartPos, mp);
                Debug.Log("distance in milimeters " + distInMilimeters);
                if(distInMilimeters > 5.0f) //if user pressed and drag pointer, dond add point
                    return;

                mp = Camera.main.ScreenToWorldPoint(mp);
                mp.z = 0.0f;
                var dt = Mathf.Abs((mp - lastPosition).magnitude);
                var md = (minimumDistance > 1.0f) ? minimumDistance : 1.0f;
                //if (Input.GetMouseButton(0) /* && dt > md */)
                {
                    var splineInxed = insertSplinePoint(mp);
                    updateAllRedDots();
                    lastPosition = mp;
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
