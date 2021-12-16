using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
    
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque; 
    public float maxSteeringAngle;
    public float rspeed = 0f;
    public float lspeed = 0f;
    public float speed = 0f;
    public float steeringAngle = 0f;
    public Transform steeringWheel;
    public XRController RightController;
    public XRController LeftController;
    public bool vrControl = false;
    public Transform SeatTransform;
    private Transform player;
    private bool isSeating = false;

    
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Seat()
    {
        if (!isSeating)
        {
            player.SetParent(SeatTransform);
            player.localPosition = Vector3.zero;
            isSeating = true;
        }
    }

    public void FixedUpdate()
    {
        if (vrControl)
        {
            steeringAngle = steeringWheel.localRotation.eulerAngles.x;
            if (steeringAngle > 180)
                steeringAngle -= 360;
            steeringAngle /= 90;
            RightController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out rspeed);
            LeftController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out lspeed);
            if (isSeating)
                speed = rspeed - lspeed;
            else
                speed = 0f;
        }

        float motor = maxMotorTorque * speed;
        float steering = maxSteeringAngle * steeringAngle;
            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering; 
}
