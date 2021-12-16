using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque; 
    public float maxSteeringAngle;
    public float speed = 0f;
    public float steeringAngle = 0f;
        
    public void FixedUpdate()
    {
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
