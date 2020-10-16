using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public UnityEvent<bool> OnUIInventoryDragEvent;

    public GameObject prefab;

    public bool Draggable { get; set; }

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private RectTransform inventoryRectTransform;
    private Transform parent;
    private bool dragScrollView = false;
    public Canvas canvas;
    public Vector2 deltaPos = new Vector2(0,0);

    public float prevHorizontalNormalizedPosition;

    [SerializeField] private ScrollRect scrollRect;
    float parentHeight = 0.0f;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        parent = rectTransform.parent;

        parentRectTransform = parent.GetComponent<RectTransform>();
        parentHeight = parentRectTransform.sizeDelta.y;

        inventoryRectTransform = parent.parent.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Draggable)
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnUIInventoryDragEvent?.Invoke(true);

        deltaPos.x = 0.0f;
        deltaPos.y = 0.0f;

        dragScrollView = true;
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
        rectTransform.SetParent(canvas.transform);
        

    }

    public void OnDrag(PointerEventData eventData)
    {
            Debug.Log("Event data pos = " + eventData.position.ToString());
            

            deltaPos += eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            // Debug.Log("deltaPos.y = " + deltaPos.y.ToString());
            // Debug.Log("parentHeight = " + parentHeight.ToString());
            // Debug.Log("dragScroll = " + dragScrollView.ToString());
            if(Mathf.Abs(deltaPos.y) > (parentHeight/8.0f))
                dragScrollView = false;


            if(dragScrollView)
                ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);

        //add new fant if drag ended outside inventory panel
        if(!rectTransform.Overlaps(inventoryRectTransform))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0), prefab.transform.rotation);
        }

        //return ui image to start position
        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = new Vector2(0,0);

        //propagate information to system that inventory drag ended
        OnUIInventoryDragEvent?.Invoke(false);
    }
}
