using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableSprite : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private float startPosX;
    private float startPoxY;
    private bool m_dragable = true;

    public UnityEvent<GameObject> OnSpritePointerDown;
    public UnityEvent<GameObject> OnSpritePointerUp;

    public UnityEvent<GameObject> OnSpriteDrag;
    public UnityEvent<GameObject> OnSpriteBeginDrag;
    public UnityEvent<GameObject> OnSpriteEndDrag;
    // Update is called once per frame
    private RigidbodyType2D m_previousBodyType = RigidbodyType2D.Static;

    void Start () 
    {

    }

    public void setDragable(bool dragable)
    {
        m_dragable = dragable;
    }

    public void OnPointerDown (PointerEventData eventData) 
    {
        Debug.Log ("OnPointerDown()");
        OnSpritePointerDown?.Invoke (this.gameObject);


        if(m_dragable)
        {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
            if (rigidbody != null) { //errors here
                m_previousBodyType = rigidbody.bodyType;
                rigidbody.bodyType = RigidbodyType2D.Kinematic;
            }

            Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);

            startPosX = worldPos.x - this.transform.localPosition.x;
            startPoxY = worldPos.y - this.transform.localPosition.y;
        }       
    }


    public void OnPointerUp (PointerEventData eventData) 
    {
        Debug.Log ("OnPointerUp()");
        OnSpritePointerUp?.Invoke (this.gameObject);
        
        if(m_dragable)
        {
            Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
            if (rigidbody != null) { //errors here
                rigidbody.bodyType = m_previousBodyType;
            }
        }
    }


    public void OnBeginDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnBeginDrag()");
        OnSpriteBeginDrag?.Invoke (this.gameObject);
    }


    public void OnDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnDrag()");

        if(m_dragable)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);
            this.gameObject.transform.localPosition = new Vector3 (worldPos.x - startPosX, worldPos.y - startPoxY, 0);
        }
        
        OnSpriteDrag?.Invoke (this.gameObject);
    }


    public void OnEndDrag (PointerEventData eventData) 
    {
        Debug.Log ("OnEndDrag()");
        OnSpriteEndDrag?.Invoke (this.gameObject);
    }

}