using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PanZoom : MonoBehaviour {
    public UnityEvent<bool> OnPanZoomActiveEvent;

    Vector3 touchStart;
    public float zoomOutMin = 0.1f;
    public float zoomOutMax = 50.0f;

    enum State {
        NONE,
        DRAG,
        ZOOM
    }
    State state = State.NONE;

    Vector2 touchDragStartPos_screen; // = new Vector2();
    Vector3 touchDragStartPos_wordl; // = new Vector3();

    Vector2 touchZoomStartPos1_screen; // = new Vector2(); 
    Vector3 touchZoomStartPos1_wordl; // = new Vector3();

    Vector2 touchDragInZoomStartPos_screen; // = new Vector2();
    Vector3 touchDragInZoomStartPos_wordl; // = new Vector3();
    Vector2 touchZoomStartPos2_screen; // = new Vector2(); 
    Vector3 touchZoomStartPos2_wordl; // = new Vector3();

    private static GameLogics gameLogics;

    void Awake () {
        if (gameLogics == null)
            gameLogics = GameObject.FindWithTag ("GameLogics").GetComponent<GameLogics> ();

    }

    void onDragStarted () {
        //Debug.Log("On DRAG"); 
        state = State.DRAG;
        gameLogics.cameraFollowEnable = false;

        //touchZeroStartPos_screen = Input.GetTouch(0).position;
        //touchZeroStartPos_wordl = Camera.main.ScreenToWorldPoint(touchZeroStartPos_screen);

    }

    void onZoomStarted () {
        //Debug.Log("On ZOOM"); 
        state = State.ZOOM;

        //touchZeroStartPos_screen = Input.GetTouch(0).position;
        //touchZeroStartPos_wordl = Camera.main.ScreenToWorldPoint(touchZeroStartPos_screen);

        //touchOneStartPos_screen = Input.GetTouch(0).position;
        //touchOneStartPos_wordl = Camera.main.ScreenToWorldPoint(touchZeroStartPos_screen);
    }

    void onStop () {
        //Debug.Log("On NONE"); 
        state = State.NONE;

    }

    void debugState () {
        //Debug.Log(state.ToString());
        //Debug.Log("Input.touchCount " + Input.touchCount.ToString());
        if (Input.touchCount == 1) {
            //print("Input.GetTouch(0).position", Input.GetTouch(0).position);
        } else if (Input.touchCount == 2) {
            //print("Input.GetTouch(0).position", Input.GetTouch(0).position);
            //print("Input.GetTouch(1).position", Input.GetTouch(1).position);      
        }

        switch (state) {
            case State.NONE:
                {

                    break;
                }
            case State.DRAG:
                {
                    //print("touchDragStartPos_screen", touchDragStartPos_screen);
                    //print("touchDragStartPos_wordl", touchDragStartPos_wordl);
                    break;
                }
            case State.ZOOM:
                {
                    //print("touchZoomStartPos1_screen", touchZoomStartPos1_screen);
                    //print("touchZoomStartPos2_screen", touchZoomStartPos2_screen);
                    break;
                }
            default:
                break;
        }
    }

    void Update () {

        // if (EventSystem.current.IsPointerOverGameObject()) //if pointer ovet UI dont make pan-zoom
        //     return;

        // if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId))
        //     return;

        //debugState();

        //---------- START PAN ZOOM -----------
        //IF ONE FINGER IS PRESSED AND NO GUI
        #warning TODO check EventSystem.current.IsPointerOverGameObject for mouse
        if ((Input.GetMouseButtonDown(0)) && (state == State.NONE) && Input.GetKey(KeyCode.Space)/*&& !EventSystem.current.IsPointerOverGameObject (Input.mousePosition)*/) {
            state = State.DRAG;
            Debug.Log ("PanZoom drag started");
            OnPanZoomActiveEvent?.Invoke (true);

            touchDragStartPos_screen = Input.mousePosition;
            touchDragStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if (((Input.touchCount == 1) && Input.GetTouch (0).phase == TouchPhase.Began) && (state == State.NONE) && !EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) {
            state = State.DRAG;
            Debug.Log ("PanZoom drag started");
            OnPanZoomActiveEvent?.Invoke (true);

            touchDragStartPos_screen = Input.GetTouch (0).position;
            touchDragStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if ((Input.touchCount == 2) && (Input.GetTouch (0).phase == TouchPhase.Began) && (Input.GetTouch (1).phase == TouchPhase.Began) && !EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId) && !EventSystem.current.IsPointerOverGameObject (Input.GetTouch (1).fingerId)) {
            state = State.ZOOM;
            OnPanZoomActiveEvent?.Invoke (true);
            touchZoomStartPos1_screen = Input.GetTouch (0).position;
            touchZoomStartPos2_screen = Input.GetTouch (1).position;

            touchDragInZoomStartPos_screen = (touchZoomStartPos1_screen + touchZoomStartPos2_screen) / 2.0f;
            touchDragInZoomStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragInZoomStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        //------------------------

        //--------- STOP PAN ZOOM -----------------

        if ((Input.GetMouseButtonUp(0)) && (state != State.NONE)) {
            state = State.NONE;

            OnPanZoomActiveEvent?.Invoke (false);

            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if (((Input.touchCount == 1) && Input.GetTouch (0).phase == TouchPhase.Ended) && (state != State.NONE)) {
            state = State.NONE;

            OnPanZoomActiveEvent?.Invoke (false);

            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if ((Input.touchCount == 2) && (Input.GetTouch (0).phase == TouchPhase.Ended) && (Input.GetTouch (1).phase == TouchPhase.Ended) && (state != State.NONE)) {
            state = State.NONE;

            OnPanZoomActiveEvent?.Invoke (false);

            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        //-----------------------------

        if (((Input.touchCount == 2) && Input.GetTouch (0).phase == TouchPhase.Began) && (state != State.NONE)) {
            state = State.ZOOM;
            touchZoomStartPos1_screen = Input.GetTouch (0).position;
            touchZoomStartPos2_screen = Input.GetTouch (1).position;

            touchDragInZoomStartPos_screen = (touchZoomStartPos1_screen + touchZoomStartPos2_screen) / 2.0f;
            touchDragInZoomStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragInZoomStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if (((Input.touchCount == 2) && Input.GetTouch (1).phase == TouchPhase.Began) && (state != State.NONE)) {
            state = State.ZOOM;
            touchZoomStartPos1_screen = Input.GetTouch (0).position;
            touchZoomStartPos2_screen = Input.GetTouch (1).position;

            touchDragInZoomStartPos_screen = (touchZoomStartPos1_screen + touchZoomStartPos2_screen) / 2.0f;
            touchDragInZoomStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragInZoomStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if (((Input.touchCount == 2) && Input.GetTouch (1).phase == TouchPhase.Ended) && (state == State.ZOOM)) {
            state = State.DRAG;
            touchDragStartPos_screen = Input.GetTouch (0).position;
            touchDragStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        if (((Input.touchCount == 2) && Input.GetTouch (0).phase == TouchPhase.Ended) && (state == State.ZOOM)) {
            state = State.DRAG;
            touchDragStartPos_screen = Input.GetTouch (1).position;
            touchDragStartPos_wordl = Camera.main.ScreenToWorldPoint (touchDragStartPos_screen);
            return; //return po to zeby przy zmianie stanu nie wplywac na transformacje poźnie i zaaktualizowac wszystko
        }

        switch (state) {
            case State.NONE:
                {

                    break;
                }
            case State.DRAG:
                {
                    if(Input.touchCount == 0) // if no touch but in DRAG mode that means that mouse activated drag
                    {
                        Vector3 direction = touchDragStartPos_wordl - Camera.main.ScreenToWorldPoint (Input.mousePosition);
                        Camera.main.transform.position += direction;
                    }
                    else
                    {
                        Vector3 direction = touchDragStartPos_wordl - Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
                        Camera.main.transform.position += direction;
                    }
                    break;
                }
            case State.ZOOM:
                {
                    Vector3 direction = touchDragInZoomStartPos_wordl - Camera.main.ScreenToWorldPoint ((Input.GetTouch (0).position + Input.GetTouch (1).position) / 2.0f);
                    Camera.main.transform.position += direction;

                    Touch touchZero = Input.GetTouch (0);
                    Touch touchOne = Input.GetTouch (1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    zoom (difference * 0.01f);
                    break;
                }
            default:
                break;
        }

        // if(Input.GetMouseButton(0)){
        //     Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Camera.main.transform.position += direction;
        // }

        // if(Input.touchCount == 2){
        //     Touch touchZero = Input.GetTouch(0);
        //     Touch touchOne = Input.GetTouch(1);

        //     Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        //     Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        //     float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        //     float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        //     float difference = currentMagnitude - prevMagnitude;

        //     zoom(difference * 0.01f);
        // }else if(Input.GetMouseButton(0)){
        //     Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Camera.main.transform.position += direction;
        // }

        // zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void print (string txt, Vector3 v) {
        Debug.Log (txt + " " + v.ToString ());
    }

    void print (string txt, Vector2 v) {
        Debug.Log (txt + " " + v.ToString ());
    }
    void zoom (float increment) {
        Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}