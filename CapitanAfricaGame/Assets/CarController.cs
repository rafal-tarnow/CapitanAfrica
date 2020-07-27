using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float fuel = 0.5f;
    public float fuelconsumption = 0.1f;
    public Rigidbody2D carRigidbody;
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
        //FUEL
        image.fillAmount = fuel;
        //DISTANCE
        DistanceText.distance = Mathf.CeilToInt(transform.position.x);
    }



    void FixedUpdate() 
    {
        float gas = getGas();
        
        if(fuel > 0.0f)
        {
            applyGasToCar(gas);

            //carRigidbody.AddTorque(-movement * carTorque * Time.fixedDeltaTime);
            fuel -= fuelconsumption*Mathf.Abs(gas)*Time.fixedDeltaTime;
        }
        else
        {
            applyGasToCar(0);
        }
        
    }    
    
    private float getGas()
    {

        if(Input.touchCount > 0) //Touch
        {
            Touch touch = Input.GetTouch(0);

            if(touch.position.x > (Screen.width/2))
            {
                return(1.0f);
            }
            else
            {
                return (-1.0f);
            }
        }
        else    //Keyboard
        {
            return(Input.GetAxis("Horizontal"));
        }
    }

    private void applyGasToCar(float x)
    {
     if(x > 0)
        {
            backMotor.motorSpeed = speedForward;
            frontMotor.motorSpeed = speedForward;

            backMotor.maxMotorTorque = Torque;
            frontMotor.maxMotorTorque = Torque;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;

        }
        else if(x < 0)
        {
            backMotor.motorSpeed = speedBackward;
            frontMotor.motorSpeed = speedBackward;

            backMotor.maxMotorTorque = Torque;
            frontMotor.maxMotorTorque = Torque;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;
        }
        else
        {
            backMotor.motorSpeed = 0;
            frontMotor.motorSpeed = 0;

            backMotor.maxMotorTorque = 1;
            frontMotor.maxMotorTorque = 1;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;                           
        }
    }
}
