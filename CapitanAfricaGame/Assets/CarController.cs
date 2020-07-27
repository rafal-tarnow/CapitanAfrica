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

    //--------------------
    private JointMotor2D backMotor, frontMotor;
    public WheelJoint2D wheelFrontJoint, wheelBackJoint;
    public float speedForward, speedBackward;
    public float Torque;

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
        // if(Input.touchCount > 0) //Touch
        // {
        //     Touch touch = Input.GetTouch(0);

        //     if(touch.position.x > (Screen.width/2))
        //     {
        //         movement = 1.0f;
        //     }
        //     else
        //     {
        //         movement = -1.0f;
        //     }
        // }
        // else    //Keyboard
        // {
        //     movement = Input.GetAxis("Horizontal");
        // }

        float x  = Input.GetAxis("Horizontal");
        if(x > 0)
        {
            backMotor.motorSpeed = speedForward;
            frontMotor.motorSpeed = speedForward;

            backMotor.maxMotorTorque = Torque;
            frontMotor.maxMotorTorque = Torque;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;

        }else if(x < 0)
        {
            backMotor.motorSpeed = speedBackward;
            frontMotor.motorSpeed = speedBackward;

            backMotor.maxMotorTorque = Torque;
            frontMotor.maxMotorTorque = Torque;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;
        }

        //FUEL
        image.fillAmount = fuel;
        //DISTANCE
        DistanceText.distance = Mathf.CeilToInt(transform.position.x);
    }

    private void FixedUpdate() {
        if(fuelconsumption > 0.0f)
        {
            //backTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            //frontTire.AddTorque(-movement * speed * Time.fixedDeltaTime);
            //carRigidbody.AddTorque(-movement * carTorque * Time.fixedDeltaTime);
        }
        fuel -= fuelconsumption*Mathf.Abs(movement)*Time.fixedDeltaTime;
    }
}
