using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableSprite : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public static GameObject gameLogics;
    private float startPosX;
    private float startPoxY;

    public UnityEvent<GameObject> OnSpriteDrag;
    public UnityEvent<GameObject> OnSpriteBeginDrag;
    public UnityEvent<GameObject> OnSpriteEndDrag;
    // Update is called once per frame

    void Start () {
        if (gameLogics == null)
            gameLogics = GameObject.FindWithTag ("GameLogics");
    }

    public void OnPointerDown (PointerEventData eventData) {
        //Debug.Log ("Coin OnPointerDown()");

        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
        if (rigidbody != null) { //errors here
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);

        startPosX = worldPos.x - this.transform.localPosition.x;
        startPoxY = worldPos.y - this.transform.localPosition.y;

        gameLogics.GetComponent<GameLogics> ().OnDragSprite (true);
        OnSpriteBeginDrag?.Invoke (this.gameObject);
    }

    public void OnPointerUp (PointerEventData eventData) {
        //Debug.Log ("Coin OnPointerUp()");

        OnSpriteEndDrag?.Invoke (this.gameObject);
        gameLogics.GetComponent<GameLogics> ().OnDragSprite (false);

        Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D> ();
        if (rigidbody != null) { //errors here
            rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }

    public void OnBeginDrag (PointerEventData eventData) {
        //Debug.Log ("Coin OnBeginDrag()");
    }

    public void OnDrag (PointerEventData eventData) {
        //Debug.Log ("Coin OnDrag()");

        Vector3 worldPos = Camera.main.ScreenToWorldPoint (eventData.position);

        this.gameObject.transform.localPosition = new Vector3 (worldPos.x - startPosX, worldPos.y - startPoxY, 0);
        OnSpriteDrag?.Invoke (this.gameObject);
    }

    public void OnEndDrag (PointerEventData eventData) {
        //Debug.Log ("Coin OnEndDrag()");
    }

}