using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EditableGround : MonoBehaviour
{
    private SpriteShapeController spriteShapeController;
    Vector3 lastPosition;
    float minimumDistance = 0.3f;
    Spline spline;

    // Start is called before the first frame update
    void Start()
    {
        spriteShapeController = gameObject.GetComponent<SpriteShapeController>();
        spline = spriteShapeController.spline;
    }


    // Update is called once per frame
    void Update()
    {
        var insertPoint = Input.mousePosition;
        insertPoint.z = 0.0f;
        insertPoint = Camera.main.ScreenToWorldPoint(insertPoint);
        var m = Mathf.Abs((insertPoint - lastPosition).magnitude);
        if (Input.GetMouseButton(0) && m > minimumDistance)
        {
            

            spline.InsertPointAt(spline.GetPointCount(), insertPoint);
            var newPointIndex = spline.GetPointCount() - 1;
            spline.SetTangentMode(newPointIndex, ShapeTangentMode.Linear);
            spline.SetHeight(newPointIndex, 1.0f);
            lastPosition = insertPoint;
            spriteShapeController.BakeCollider();
        }
    }
}
