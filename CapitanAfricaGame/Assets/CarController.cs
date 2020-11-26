using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float fuel = 1.0f;
    private float previousFuel = 1.0f;
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
        if(previousFuel - fuel > 0.1) // optymalization, dont update fuel image with small amount
        {
            previousFuel = fuel;
            image.fillAmount = fuel;
        }
        //DISTANCE
        //DistanceText.setDistance((int)(transform.position.x));
        DistanceTextMP.setDistance((int)(transform.position.x));
    }

    public void setFuel(float f)
    {
        fuel = f;
        previousFuel = fuel;
        image.fillAmount = fuel;
    }

    void FixedUpdate() 
    {   

        getGas();

        if(fuel > 0.0f)
        {
            fuel -= fuelconsumption*Time.fixedDeltaTime;
        }
        else
        {
            applyGasToCar(0);
        }
        
    }    
    
    private bool keySlashPressed = false;
    private bool keyZPressed = false;

    private void getGas()
    {     
        if(Input.GetKeyDown(KeyCode.Slash))
        {
            keySlashPressed = true;

            if(!keyZPressed)
                setGas(1.0f);
            else
                setGas(1.0f);
        }

        if(Input.GetKeyUp(KeyCode.Slash))
        {
            keySlashPressed = false;

            if(!keyZPressed)
                setGas(0.0f);
            else
                setGas(-1.0f);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            keyZPressed = true;

            if(!keySlashPressed)
                setGas(-1.0f);
            else
                setGas(-1.0f);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            keyZPressed = false;

            if(!keySlashPressed)
                setGas(0.0f);
            else
                setGas(1.0f);
        }
    }

    public void setGas(float gas)
    {
        if(fuel>0.0f)
            applyGasToCar(gas);
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
