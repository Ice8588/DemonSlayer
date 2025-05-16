using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Kinect = Windows.Kinect;


public class BodyState : MonoBehaviour
{
    private BodySourceManager _BodyManager;
    private bool _detectBodyDirection {get; set;} = false;

    // Body direction
    public bool _isCatch {get; private set;} = false;
    public int _moveH {get; private set;} = 0;
    public int _moveV {get; private set;} = 0;
    public int _turn {get; private set;} = 0;
    public float _handDegree {get; private set;} = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (_BodyManager == null)
        {
            _BodyManager = FindObjectOfType<BodySourceManager>();
        }

        if (_BodyManager != null)
        {
            Kinect.Body[] data = _BodyManager.GetData();

            if (_BodyManager.GetBodyCount() == 0 || data == null)
            {
                _detectBodyDirection = false;
                _isCatch = false;
                _moveH = 0;
                _moveV = 0;
                _turn = 0;
                _handDegree = 0;
            }
            else
            {

                foreach (Kinect.Body body in data)
                {
                    if (body.IsTracked)
                    {
                        _detectBodyDirection = true;
                        // Get the joints
                        JointModel chest = new JointModel(body.Joints[Kinect.JointType.SpineMid]);
                        JointModel hadnLeft = new JointModel(body.Joints[Kinect.JointType.HandTipLeft]);
                        JointModel hadnRight = new JointModel(body.Joints[Kinect.JointType.HandTipRight]);
                        JointModel shoulderLeft = new JointModel(body.Joints[Kinect.JointType.ShoulderLeft]);
                        JointModel shoulderMid = new JointModel(body.Joints[Kinect.JointType.SpineShoulder]);
                        JointModel shoulderRight = new JointModel(body.Joints[Kinect.JointType.ShoulderRight]);
                        JointModel spineBase = new JointModel(body.Joints[Kinect.JointType.SpineBase]);
                        JointModel neck = new JointModel(body.Joints[Kinect.JointType.Neck]);
                        JointModel head = new JointModel(body.Joints[Kinect.JointType.Head]);

                        // Detect the body direction
                        DetectCatch(hadnLeft, hadnRight);
                        DetectBodyDirection(head, neck, spineBase);
                        DetectHandDegree(chest, hadnLeft, hadnRight);
                        DetectTurn(shoulderLeft, shoulderMid, shoulderRight);


                        break;
                    }
                }
            }
        }
        
    }

    void debugFunction()
    {
        GameObject.Find("State").GetComponent<TextMesh>().text = "Hand Degree: " + _handDegree + "\n" +
        "Move H: " + _moveH + "\n" + "Move V: " + _moveV + "\n" +
        "Turn: " + _turn + "\n" + "Is Catch: " + _isCatch + "\n" +
        "Is Detect: " + _detectBodyDirection + "\n";
    }


    void DetectCatch(JointModel hadnLeft, JointModel hadnRight)
    {
        _isCatch = Math.Abs(hadnLeft.Position.x - hadnRight.Position.x) < 0.125f &&
                   Math.Abs(hadnLeft.Position.y - hadnRight.Position.y) < 0.125f;
    }
    
    void DetectHandDegree(JointModel chest, JointModel handLeft, JointModel handRight)
    {
        if(!_isCatch) return;

        Vector2 handAverage = new Vector2(
        (handLeft.Position.x + handRight.Position.x) / 2,
        (handLeft.Position.y + handRight.Position.y) / 2
        );

        Vector2 chestPosition = new Vector2(chest.Position.x, chest.Position.y);

        float degree = Mathf.Atan2(handAverage.y - chestPosition.y, handAverage.x - chestPosition.x) * Mathf.Rad2Deg;

        if (Vector2.Distance(handAverage, chestPosition) < 0.2f) return;
        
        _handDegree = degree < 0 ? 360 + degree : degree;
    }

    void DetectTurn(JointModel shoulderLeft, JointModel shoulderMid, JointModel shoulderRight)
    {
        float angle = Mathf.Atan2(shoulderLeft.Position.z - shoulderMid.Position.z, shoulderLeft.Position.x - shoulderMid.Position.x) * Mathf.Rad2Deg;
        angle = angle < 0 ? 360 + angle : angle;
        
        if (angle >= 90 && angle <= 150) _turn = -1;
        else if (angle >= 245 && angle <= 280) _turn = 1;
        else _turn = 0;
    }

    void DetectBodyDirection(JointModel head, JointModel neck, JointModel spineBase)
    {
        // Z axis
        float angleBaseNeck = Mathf.Atan2(neck.Position.z - spineBase.Position.z, neck.Position.x - spineBase.Position.x) * Mathf.Rad2Deg;
        float angleNeckHead = Mathf.Atan2(head.Position.z - neck.Position.z, head.Position.x - neck.Position.x) * Mathf.Rad2Deg;
        if (angleBaseNeck < 0) angleBaseNeck += 360;
        if (angleNeckHead < 0) angleNeckHead += 360;

        // X axis
        float degreeX = Mathf.Atan2(head.Position.x - spineBase.Position.x, head.Position.y - spineBase.Position.y) * Mathf.Rad2Deg;
        if (degreeX < 0) degreeX += 360;

        _moveV = 0;
        _moveH = 0;
        if (angleBaseNeck > 250 && angleBaseNeck < 290) _moveV = 1;
        else if (angleNeckHead > 75 && angleNeckHead < 100) _moveV = -1;

        if (degreeX > 10 && degreeX < 20) _moveH = 1;
        else if (degreeX > 340 && degreeX < 350) _moveH = -1;

    }
}
