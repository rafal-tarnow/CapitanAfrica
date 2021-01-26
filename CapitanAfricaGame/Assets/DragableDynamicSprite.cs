using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableDynamicSprite : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private float startPosX;
    private float startPosY;

    private float worldStartPosX;
    private float worldStartPosY;

    private bool m_dragable = true;

    public UnityEvent<GameObject> OnSpritePointerDown;
    public UnityEvent<GameObject> OnSpritePointerUp;

    public UnityEvent<GameObject> OnSpriteDrag;
    public UnityEvent<GameObject> OnSpriteBeginDrag;
    public UnityEvent<GameObject> OnSpriteEndDrag;
    // Update is called once per frame
    private RigidbodyType2D m_previousBodyType = RigidbodyType2D.Static;
    private Rigidbody2D rigidBody;

    private bool m_isPointerDown = false;
    private bool m_isDragging = false;

    private Vector2 accumForce = new Vector2(0,0);


    void Start () 
    {
        if(rigidBody == null)
            rigidBody = this.GetComponent<Rigidbody2D>();
    }

    private void Update() 
    {
       Debug.Log("Is Pointer down = " + m_isPointerDown.ToString());
       Debug.Log("Is Dragging = " + m_isDragging.ToString());

       if(m_isDragging)
       {
           Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            //this.gameObject.transform.localPosition = new Vector3 (worldPos.x - startPosX, worldPos.y - startPosY, 0);

            Vector2 forceDir = new Vector2(worldPos.x, worldPos.y) - rigidBody.position;

            Debug.Log("forceDir.y = " + forceDir.y.ToString());
            float forceY = 0.0f;

            if(forceDir.y > 0.0f)
                forceY = forceDir.y*10.0f;
            else
                forceY = 0.0f;
            //


           Vector2 force  = new Vector2(0, forceY);

            rigidBody.AddForce(force, ForceMode2D.Force);
       }
    }
    

    public void setDragable(bool dragable)
    {
        m_dragable = dragable;
    }

    public void OnPointerDown (PointerEventData eventData) 
    {
        Debug.Log ("OnPointerDown()");
        m_isPointerDown = true;
        OnSpritePointerDown?.Invoke (this.gameObject);


        if(m_dragable)
        {
            // Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
            // if (rigidbody != null) { //errors here
            //     m_previousBodyType = rigidbody.bodyType;
            //     rigidbody.bodyType = RigidbodyType2D.Kinematic;
            // }

            Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);

            worldStartPosX = worldPos.x;
            worldStartPosY = worldPos.y;

            startPosX = worldPos.x - this.transform.localPosition.x;
            startPosY = worldPos.y - this.transform.localPosition.y;
        }       
    }


    public void OnPointerUp (PointerEventData eventData) 
    {
        Debug.Log ("OnPointerUp()");
        OnSpritePointerUp?.Invoke (this.gameObject);
        
        if(m_dragable)
        {
            // Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
            // if (rigidbody != null) { //errors here
            //     rigidbody.bodyType = m_previousBodyType;
            // }
        }

        m_isPointerDown = false;
    }


    public void OnBeginDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnBeginDrag()");
        m_isDragging = true;
        OnSpriteBeginDrag?.Invoke (this.gameObject);
    }


    public void OnDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnDrag()");

        if(m_dragable)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);
            //this.gameObject.transform.localPosition = new Vector3 (worldPos.x - startPosX, worldPos.y - startPosY, 0);
            //rigidBody.AddForceAtPosition()
            Debug.DrawLine(new Vector3(worldStartPosX, worldStartPosY), new Vector3(this.gameObject.transform.localPosition.x ,this.gameObject.transform.localPosition.y));  
        }
        
        OnSpriteDrag?.Invoke (this.gameObject);
    }


    public void OnEndDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnEndDrag()");
        OnSpriteEndDrag?.Invoke (this.gameObject);
        m_isDragging = false;
    }

}
