using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableSprite : MonoBehaviour/*, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler*/
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

    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     Debug.Log("Coin OnPointerDown()");
    // }

    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Coin OnBeginDrag()");
    // }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Coin OnDrag()");
    //     Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
    //     worldPosition.z = 0.0f;
    //     this.gameObject.transform.localPosition = worldPosition;
    // }
    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Coin OnEndDrag()");
    // }


}
