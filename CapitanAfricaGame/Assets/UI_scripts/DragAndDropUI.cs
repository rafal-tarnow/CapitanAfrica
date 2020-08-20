using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool Draggable { get; set; }

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Transform parent;
    private bool draggingSlot;
    private bool dragScrollView = false;
    public Canvas canvas;
    public Vector2 deltaPos = new Vector2(0,0);

    public float prevHorizontalNormalizedPosition;

    [SerializeField] private ScrollRect scrollRect;
    float parentHeight = 0.0f;

private void Awake() {
    rectTransform = GetComponent<RectTransform>();
    parent = rectTransform.parent;

    parentRectTransform = parent.GetComponent<RectTransform>();
    parentHeight = parentRectTransform.sizeDelta.y;
    
}
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Draggable)
        {
            return;
        }

        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(0.5f);
        draggingSlot = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        deltaPos.x = 0.0f;
        deltaPos.y = 0.0f;

        dragScrollView = true;
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
        rectTransform.SetParent(canvas.transform);
        

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingSlot)
        {
            //DO YOUR DRAGGING HERE
        } else
        {
            //OR DO THE SCROLLRECT'S
            deltaPos += eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Debug.Log("deltaPos.y = " + deltaPos.y.ToString());
            Debug.Log("parentHeight = " + parentHeight.ToString());
            Debug.Log("dragScroll = " + dragScrollView.ToString());
            if(Mathf.Abs(deltaPos.y) > (parentHeight/8.0f))
                dragScrollView = false;


            if(dragScrollView)
                ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);


        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
           rectTransform.SetParent(parent);
           rectTransform.anchoredPosition = new Vector2(0,0);



        if (draggingSlot)
        {
            //END YOUR DRAGGING HERE
         
            draggingSlot = false;
        }
    }
}