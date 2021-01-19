using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class EditableGround : MonoBehaviour
{

    private SpriteShapeController spriteShapeController;
    Vector3 lastPosition;
    float minimumDistance = 0.3f;
    Spline spline;
    bool dragRedDotMode = false;
    public GameObject redDotPrefab;
    private Vector3 touchStartPos;
     private GameObject groundEditable;

    List<GameObject> redDotList = new List<GameObject>();
    Dictionary<GameObject, int> redDotMap = new Dictionary<GameObject, int>();

    public enum Mode{
        PLAY,
        EDIT_INACTIVE,
        EDIT_INACTIVE_DARK,
        EDIT_DRAG_ADD_POINTS,
        EDIT_DELETE_POINTS
    }

    Mode m_mode = Mode.PLAY;


    void Awake()
    {
        if (groundEditable == null)
            groundEditable = GameObject.FindWithTag ("GroundEditable");

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

    private void dragableAllRedDots(bool dragable)
    {
        foreach(var obj in redDotList)
        {
            obj.GetComponent<DragableSprite>().setDragable(dragable);
        }
    }


    private void updateAllRedDots()
    {
        destroyAllRedDots();

        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            GameObject cloneRedDot = Instantiate(redDotPrefab) as GameObject;
            cloneRedDot.transform.position = spline.GetPosition(i);

            cloneRedDot.GetComponent<DragableSprite>().OnSpriteBeginDrag.AddListener(onRedDotBeginDrag);
            cloneRedDot.GetComponent<DragableSprite>().OnSpriteDrag.AddListener(onRedDotDrag);
            cloneRedDot.GetComponent<DragableSprite>().OnSpriteEndDrag.AddListener(onRedDotEndDrag);
            cloneRedDot.GetComponent<DragableSprite>().OnSpritePointerDown.AddListener(onRedDotPointerDown);
            cloneRedDot.GetComponent<DragableSprite>().OnSpritePointerUp.AddListener(onRedDotPointerUp);

            redDotList.Add(cloneRedDot);
            redDotMap.Add(cloneRedDot, i);
        }

    }

    private void deleteRedPoint()
    {

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


    public void onRedDotPointerDown(GameObject gameObject )
    {
        dragRedDotMode = true;
    }

    public void onRedDotPointerUp(GameObject gameObject) 
    {
        dragRedDotMode = false;

        if(m_mode == Mode.EDIT_DELETE_POINTS)
        {
            spline.RemovePointAt(redDotMap[gameObject]);
            updateAllRedDots();
            dragableAllRedDots(false);
            spriteShapeController.BakeCollider();
        }    
    }

    public void onRedDotBeginDrag(GameObject gameObject)
    {
        dragRedDotMode = true;
    }


    public void onRedDotEndDrag(GameObject gameObject)
    {
        dragRedDotMode = false;
    }


    public void onRedDotDrag(GameObject gameObject)
    {
        //Debug.Log("Red dot pos changed = " + position.ToString());
        if(m_mode == Mode.EDIT_DRAG_ADD_POINTS)
        {
            Int32 index = redDotMap[gameObject];
            Vector3 position = gameObject.transform.localPosition;

            changeSplinePointPosition(index, position);
        }
    }


    public void setMode(Mode mode) 
    {
        m_mode = mode;

        switch(m_mode)
        {
            case Mode.PLAY:
                groundEditable.SetActive (true);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1.0f);
                groundEditable.GetComponent<EditableGround> ().enabled = false; // in play mode edition is disabled
            break;
            case Mode.EDIT_INACTIVE:
            //common edit
                groundEditable.SetActive (true);
                groundEditable.GetComponent<EditableGround> ().enabled = true;
            //
                activateAllRedDots(false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1.0f);
            break;
            case Mode.EDIT_INACTIVE_DARK:
                //common edit
                groundEditable.SetActive (true);
                groundEditable.GetComponent<EditableGround> ().enabled = true;
                //
                 activateAllRedDots(false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 0.8f);
            break;
            case Mode.EDIT_DRAG_ADD_POINTS:
            //common edit
                groundEditable.SetActive (true);
                groundEditable.GetComponent<EditableGround> ().enabled = true;
            //
                activateAllRedDots(true);
                dragableAllRedDots(true);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 1f);
            break;
            case Mode.EDIT_DELETE_POINTS:
            //common edit
                groundEditable.SetActive (true);
                groundEditable.GetComponent<EditableGround> ().enabled = true;
            //
                 activateAllRedDots(true);
                 dragableAllRedDots(false);
                groundEditable.GetComponent<UnityEngine.U2D.SpriteShapeRenderer> ().color = new Color (1f, 1f, 1f, 0.8f);
            break;
        }
    }


    
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

        if((m_mode == Mode.EDIT_DRAG_ADD_POINTS) ||  (m_mode == Mode.EDIT_DELETE_POINTS))
            activateAllRedDots(true);
        else
            activateAllRedDots(false);
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
        if(m_mode != Mode.EDIT_DRAG_ADD_POINTS)
            return;

        if (IsPointerOverUIObject())
            return;


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
    }
}
