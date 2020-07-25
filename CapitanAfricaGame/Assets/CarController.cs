using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float fuel = 1.0f;
    public float fuelconsumption = 0.1f;
    public Rigidbody2D carRigidbody;
    public Rigidbody2D backTire;
    public Rigidbody2D frontTire;
    public float speed = 20;
    public float carTorque = 10;
    private float movement;
    public UnityEngine.UI.Image image;
    // Start is called before the first frame update
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    public void resetPosition()
    {
        transform.position = startPosition;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Car controller update!!");
        if(Input.touchCount > 0) //Touch
        {
            Debug.Log("   1");
            Touch touch = Input.GetTouch(0);

            if(touch.position.x > (Screen.width/2))
            {
                movement = 1.0f;
            }
            else
            {
                movement = -1.0f;
            }
        }
        else    //Keyboard
        {
            Debug.Log("   2");
            movement = Input.GetAxis("Horizontal");
        }

       


        image.fillAmount = fuel;

        DistanceText.distance = Mathf.CeilToInt(transform.position.x);
    }

    private void FixedUpdate() {
        if(fuelconsumption > 0.0f)
        {
            backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            carRigidbody.AddTorque(-movement * carTorque * Time.fixedDeltaTime);
        }
        fuel -= fuelconsumption*Mathf.Abs(movement)*Time.fixedDeltaTime;
    }
}
