using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleCarController : MonoBehaviour
{
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
    public Transform StandTransform;
    private Transform player;
    private bool isSeating = false;
    private AudioSource audio;
    private float globalVolume = 0f;
    public Transform volumeRot;
    public AudioSource radio;
    private float volumeRotationLevel = 0f;
    private bool carBreak = true;


    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
        globalVolume = PlayerPrefs.GetFloat("volume");
    }

    public void Seat()
    {
        if (!isSeating)
        {
            player.SetParent(SeatTransform);
            player.localPosition = Vector3.zero;
            player.localRotation = Quaternion.identity;
            isSeating = true;
            carBreak = true;
            if (radio)
                radio.volume = volumeRotationLevel * globalVolume;
        }
    }

    public void GoOut()
    {
        if (isSeating)
        {
            player.SetParent(StandTransform);
            player.localPosition = Vector3.zero;
            player.parent = null;
            isSeating = false;
            carBreak = false;
            if (audio)
                audio.volume = 0f;
            if (radio)
                radio.volume = 0f;
        }
    }
    
    public void FixedUpdate()
    {
        if (volumeRot)
        {

            float volRot = volumeRot.localRotation.eulerAngles.y;
            if (volRot >= 180)
            {
                volRot -= 360;
            }

            // Debug.Log(volRot);
            volRot += 100;
            volRot /= 200;
            
            volumeRotationLevel = volRot;

            if (radio && isSeating)
                radio.volume = volumeRotationLevel * globalVolume;
        }

        if (vrControl)
        {
            steeringAngle = steeringWheel.localRotation.eulerAngles.x;
            if (steeringAngle > 180)
                steeringAngle -= 360;
            steeringAngle /= 90;
            RightController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out rspeed);
            LeftController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out lspeed);
            bool btn = false;
            if(isSeating)
                LeftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out btn);
            carBreak = !btn;
            if (isSeating)
                speed = rspeed - lspeed;
            else
                speed = 0f;
        }
        if(audio)
            audio.volume = Mathf.Max(0.2f * globalVolume, speed * globalVolume);

        float motor = maxMotorTorque * speed;
        float steering = maxSteeringAngle * steeringAngle;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            if (carBreak)
            {
                axleInfo.leftWheel.brakeTorque = 0f;
                axleInfo.rightWheel.brakeTorque = 0f;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 1f;
                axleInfo.rightWheel.brakeTorque = 1f;
            }

            axleInfo.ApplyLocalPositionToVisuals();
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public GameObject leftWheelVisuals;
    private bool leftGrounded = false;
    private float travelL = 0f; //?????????????????? ???? ?????????????? ???????????????????? ???? ?????????????????? ??????????????????, ????????????????

    public WheelCollider rightWheel;
    public GameObject rightWheelVisuals;
    private bool rightGrounded = false;
    private float travelR = 0f; //?????????????????? ???? ?????????????? ???????????????????? ???? ?????????????????? ??????????????????, ????????????????

    public bool motor;
    public bool steering;

    public float Antiroll = 10000; //?????????????????? ??????????????????????????  
    private float AntrollForce = 0;

    public void ApplyLocalPositionToVisuals()
    {
        //left wheel      
        if (leftWheelVisuals == null)
        {
            return;
        }

        Vector3 position;
        Quaternion rotation;
        leftWheel.GetWorldPose(out position, out rotation);

        leftWheelVisuals.transform.position = position;
        leftWheelVisuals.transform.rotation = rotation;

        //right wheel  
        if (rightWheelVisuals == null)
        {
            return;
        }

        rightWheel.GetWorldPose(out position, out rotation);

        rightWheelVisuals.transform.position = position;
        rightWheelVisuals.transform.rotation = rotation;
    }

    public void CalculateAndApplyAntiRollForce(Rigidbody theBody) //???????????????????????? ??????????????????????????  
    {
        WheelHit hit;
// ?? ???????????? ?????????????? ???????????? ????????????????      
        leftGrounded = leftWheel.GetGroundHit(out hit);
        if (leftGrounded)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) /
                      leftWheel.suspensionDistance;
        else
            travelL = 1f;
        rightGrounded = rightWheel.GetGroundHit(out hit);
        if (rightGrounded)
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) /
                      rightWheel.suspensionDistance;
        else
            travelR = 1f;
// ????????, ?????????????? ?????????? ???????????? ????????????????????????????      
        AntrollForce =
            (travelL - travelR) * Antiroll; // (travelL-travelR) ???????? ?????? ???????? ?????? ???????????????????? ????????????????          
//?????????????????????? ????????       
        if (leftGrounded)
            theBody.AddForceAtPosition(leftWheel.transform.up * -AntrollForce, leftWheel.transform.position);
        if (rightGrounded)
            theBody.AddForceAtPosition(rightWheel.transform.up * AntrollForce, rightWheel.transform.position);
    }
}