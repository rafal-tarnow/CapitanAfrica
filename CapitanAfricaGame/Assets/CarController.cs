using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float fuel = 0.5f;
    public float fuelconsumption = 0.01f;
    public Rigidbody2D carRigidbody;

    public UnityEngine.UI.Image image;
    // Start is called before the first frame update
    private Vector3 startPosition;

    //--------------------
    private JointMotor2D backMotor, frontMotor;
    public WheelJoint2D wheelFrontJoint, wheelBackJoint;
    public float speedForward, speedBackward, speedFree;
    public float torqueForward;
    public float torqueBackward;
    public float torqueFree;

    private static CustomButton buttonGas;
    private static CustomButton buttonBrake;

    void Awake() 
    {
        if (buttonGas == null)
            buttonGas = GameObject.FindWithTag("ButtonGas").GetComponent<CustomButton>();
        
        if (buttonBrake == null)
            buttonBrake = GameObject.FindWithTag("ButtonBrake").GetComponent<CustomButton>();
        
    }

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

            fuel -= fuelconsumption*Mathf.Abs(gas)*Time.fixedDeltaTime;
        }
        else
        {
            applyGasToCar(0);
        }
        
    }    
    
    private float getGas()
    {
        if(buttonGas.buttonPressed)
            return 1.0f;
        if(buttonBrake.buttonPressed)
            return -1.0f;
        return 0.0f;

        //return(Input.GetAxis("Horizontal"));
    }

    private void applyGasToCar(float x)
    {
     if(x > 0)
        {
            backMotor.motorSpeed = speedForward;
            frontMotor.motorSpeed = speedForward;

            backMotor.maxMotorTorque = torqueForward;
            frontMotor.maxMotorTorque = torqueForward;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;

        }
        else if(x < 0)
        {
            backMotor.motorSpeed = speedBackward;
            frontMotor.motorSpeed = speedBackward;

            backMotor.maxMotorTorque = torqueBackward;
            frontMotor.maxMotorTorque = torqueBackward;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;
        }
        else
        {
            backMotor.motorSpeed = speedFree;
            frontMotor.motorSpeed = speedFree;

            backMotor.maxMotorTorque = torqueFree;
            frontMotor.maxMotorTorque = torqueFree;

            wheelFrontJoint.motor = frontMotor;
            wheelBackJoint.motor = backMotor;                           
        }
    }
}
