using UnityEngine;
using UnityEditor;

public class PositionExample : EditorWindow
{
    Vector2Int p1;
    bool showBtn = true;

    private Vector3 mousePosition;

    [MenuItem("Examples/position")]
    static void Init()
    {
        GetWindow<PositionExample>("position");
    }

void OnSceneGUI()
{  
   mousePosition = Event.current.mousePosition;
   mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
   mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
   mousePosition.y = -mousePosition.y;
}

    void OnGUI()
    {



        
        Rect r = position;
        GUILayout.Label("Position: " + mousePosition.x + "x" + mousePosition.y);

        p1 = EditorGUILayout.Vector2IntField("Set the position:", p1);
        if (showBtn)
        {
            if (GUILayout.Button("Accept new position"))
            {
                r.x = p1.x;
                r.y = p1.y;

                position = r;
            }
        }
    }
}