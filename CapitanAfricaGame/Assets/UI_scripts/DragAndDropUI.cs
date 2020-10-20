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
    private RectTransform childImageRectTransform;
    private RectTransform parentRectTransform;
    private RectTransform inventoryRectTransform;
    private Transform parent;
    private bool dragScrollView = false;
    public Canvas canvas;
    public Vector2 deltaPos = new Vector2(0,0);

    public float prevHorizontalNormalizedPosition;
    public bool newInstanceCreated = false;
    private GameObject newObject;
    private float newObjectWidht;
    private float newObjectHeight;

    [SerializeField] private ScrollRect scrollRect;
    float parentHeight = 0.0f;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        parent = rectTransform.parent;

        parentRectTransform = parent.GetComponent<RectTransform>();
        parentHeight = parentRectTransform.sizeDelta.y;

        inventoryRectTransform = parent.parent.parent.GetComponent<RectTransform>();

        childImageRectTransform = transform.GetChild(0).GetComponent<RectTransform>();

        newInstanceCreated = false;
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



        if(!childImageRectTransform.Overlaps(inventoryRectTransform))
        {
            this.transform.GetChild(0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.0f);

            if(newInstanceCreated == false)
            {
                newInstanceCreated = true;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
                newObject = Instantiate(prefab, new Vector3(worldPos.x, worldPos.y, 0), prefab.transform.rotation);

                newObjectWidht = newObject.GetComponent<Renderer>().bounds.size.x;
                newObjectHeight = newObject.GetComponent<Renderer>().bounds.size.y;
            }
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1.0f);

            if(newInstanceCreated == true)
            {
                newInstanceCreated = false;
                Destroy(newObject);
            }
        }

        if(newInstanceCreated)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);
            newObject.transform.localPosition = new Vector3 (worldPos.x - newObjectWidht/2.0f, worldPos.y + newObjectHeight/2.0f, 0);
        }


    }

    public void OnEndDrag(PointerEventData eventData)
    {

        newInstanceCreated = false;

        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);

        //return ui image to start position
        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = new Vector2(0,0);
        this.transform.GetChild(0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 1.0f);

        //propagate information to system that inventory drag ended
        OnUIInventoryDragEvent?.Invoke(false);
    }
}
