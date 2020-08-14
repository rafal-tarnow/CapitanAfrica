using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragableSprite : MonoBehaviour
{
    private float startPosX;
    private float startPoxY;
    private bool isBeginHeld = false;

    public UnityEvent<GameObject> OnPositionChanged;
    // Update is called once per frame
    void Update()
    {
        if(isBeginHeld == true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPoxY, 0);
            OnPositionChanged?.Invoke(this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Coin Mouse Down");
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPoxY = mousePos.y - this.transform.localPosition.y;

            isBeginHeld = true;
        }
    }

    private void OnMouseUp()
    {
        Debug.Log("Coin Mouse Up");
        isBeginHeld = false;
    }
}
