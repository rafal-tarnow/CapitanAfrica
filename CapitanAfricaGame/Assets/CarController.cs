﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float fuel = 1.0f;
    private float previousFuel = 1.0f;
    public float fuelconsumption = 0.01f;
    public Rigidbody2D carRigidbody;
    private Rigidbody2D frontTireRigidbody;
    private Rigidbody2D backTireRigidbody;

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

    private float m_currentGas = 0.0f;
    bool m_gasPedalPressed = false;
    bool m_brakePedalPressed = false;
    bool m_isColliding = false;

    private AudioSource engineSound;
    private Collider2D[] childColliders;


    void Start()
    {
        startPosition = transform.position;

        if(engineSound == null)
            engineSound = this.GetComponent<AudioSource>();

        if(frontTireRigidbody == null)
            frontTireRigidbody = GameObject.FindWithTag("FrontTire").GetComponent<Rigidbody2D>();

        if(backTireRigidbody == null)
            backTireRigidbody = GameObject.FindWithTag("BackTire").GetComponent<Rigidbody2D>();

        //engineSound.Play();
        childColliders = this.GetComponentsInChildren<Collider2D>();
    }


    public void onGasPedalPressedEvent(bool pressed)
    {
        m_gasPedalPressed = pressed;

        if(m_gasPedalPressed && m_brakePedalPressed)
        {
            this.setGasEvent(1.0f);
            return;
        }
        if(m_gasPedalPressed && !m_brakePedalPressed)
        {
            this.setGasEvent(1.0f);
            return;
        }
        if(!m_gasPedalPressed && m_brakePedalPressed)
        {
            this.setGasEvent(-1.0f);
            return;
        }
        if(!m_gasPedalPressed && !m_brakePedalPressed)
        {
            this.setGasEvent(0.0f);
            return;
        }
    }


    public void onBrakePedalPressedEvent(bool pressed) 
    {
        m_brakePedalPressed = pressed;

        if(m_brakePedalPressed && m_gasPedalPressed)
        {
            this.setGasEvent(-1.0f);
            return;
        }
        if(m_brakePedalPressed && !m_gasPedalPressed)
        {
            this.setGasEvent(-1.0f);
            return;
        }
        if(!m_brakePedalPressed && m_gasPedalPressed)
        {
            this.setGasEvent(1.0f);
            return;
        }
        if(!m_brakePedalPressed && !m_gasPedalPressed)
        {
            this.setGasEvent(0.0f);
            return;
        }
    }


    public void resetPosition()
    {
        transform.position = startPosition;
    }


    private void setGasEvent(float gas)
    {
        // if(gas > 0.0f)
        // {
        //     carRigidbody.AddTorque(-5, ForceMode2D.Force);
        // }
        // else if(gas < 0.0f)
        // {
        //     carRigidbody.AddTorque(5, ForceMode2D.Force);
        // }
        // else
        // {
        //     //carRigidbody.AddTorque(0, ForceMode2D.Force);
        // }
        

        m_currentGas = gas;
        if(fuel>0.0f)
            applyGasToCar(gas);
    }


    private void FixedUpdate() 
    {
        //carRigidbody.rotation += 200.0f*Time.fixedDeltaTime;
        //carRigidbody.AddTorque(-5, ForceMode2D.Force);
        Debug.Log("Is child colliding = " + areChildsCollide().ToString());


        #warning przerobic rotate z pool na event
        if(areChildsCollide())
        {
            carRigidbody.AddTorque(0, ForceMode2D.Force);
        }
        else
        {
            if(m_currentGas > 0.0f)
            {
                if(carRigidbody.angularVelocity < 70)
                    carRigidbody.AddTorque(500*Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else if(m_currentGas < 0.0f)
            {
                if(carRigidbody.angularVelocity > -70)
                    carRigidbody.AddTorque(-500*Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }

    }
    

    void Update()
    {
        // Debug.Log("FrontTireAngularVelocity = " + frontTireRigidbody.angularVelocity.ToString());

        // float speedForwardAbs = Mathf.Abs(frontTireRigidbody.angularVelocity);
        // float speedBackwardAbs = Mathf.Abs(backTireRigidbody.angularVelocity);

        // float speedRatio;
        // if(speedForwardAbs < speedBackwardAbs)
        // {
        //     speedRatio = (speedForwardAbs/-speedForward)*3.0f;
        // }
        // else
        // {
        //    speedRatio = (speedBackwardAbs/-speedForward)*3.0f;
        // }
        

        // Debug.Log("SpeedRatio = " + speedRatio.ToString());

        // engineSound.pitch = speedRatio + 0.6f;

        if(fuel > 0.0f)
        {
            fuel -= fuelconsumption*Time.fixedDeltaTime;
        }
        else
        {
            applyGasToCar(0);
        }

        getGasFromKeyboard();
        //FUEL
        if(previousFuel - fuel > 0.1) // optymalization, dont update fuel image with small amount
        {
            previousFuel = fuel;
            image.fillAmount = fuel;
        }

        DistanceTextMP.setDistance((int)(transform.position.x));
    }

    public void setFuel(float f)
    {
        fuel = f;
        previousFuel = fuel;
        image.fillAmount = fuel;
    }
    

    private void getGasFromKeyboard()
    {     
        if(Input.GetKeyDown(KeyCode.Slash))
        {
            onGasPedalPressedEvent(true);
        }

        if(Input.GetKeyUp(KeyCode.Slash))
        {
            onGasPedalPressedEvent(false);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            onBrakePedalPressedEvent(true);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            onBrakePedalPressedEvent(false);
        }
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

    bool areChildsCollide()
    {
        for(int i = 0; i < childColliders.Length; i++)
        {
            if(childColliders[i].IsTouchingLayers(Physics2D.AllLayers))
                return true;
        }
        return false;
    }

    // public void OnTriggerEnter2D(Collider2D other) {
    //     //Debug.Log("Car controller: OnTriggerEnter2D");
    // }

    // public void OnTriggerStay(Collider other) {
    //     //Debug.Log("Car controller: OnTriggerStay");
    // }

    // public void OnTriggerExit2D(Collider2D other)
    // {
    //     //Debug.Log("Car controller: OnTriggerExit2D");
    // }
    
    // public void OnCollisionEnter2D(Collision2D other) {
    //     //Debug.Log("Car controller: OnCollisionEnter2D");
    //     m_isColliding = true;
    // }

    // public void OnCollisionStay2D(Collision2D other) {
    //     //Debug.Log("Car controller: OnCollisionStay2D");
    // }

    // public void OnCollisionExit2D(Collision2D other) {
    //     //Debug.Log("Car controller: OnCollisionExit2D");
    //     m_isColliding = false;
    // }
}
