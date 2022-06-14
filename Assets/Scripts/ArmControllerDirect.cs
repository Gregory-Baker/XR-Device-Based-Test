using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.KortexDriver;
using Valve.VR;
using System;

public class ArmControllerDirect : MonoBehaviour
{
    ROSConnection ros;

    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    public SteamVR_Action_Vector2 xyInput;
    public SteamVR_Action_Boolean enableMove;
    public SteamVR_Action_Vector2 scroll;

    [SerializeField]
    string topicName = "/kinova_arm/in/cartesian_velocity";

    [SerializeField]
    float maxLinearVel = 0.1f;

    [SerializeField]
    float maxAngularVel = 0.4f;

    [SerializeField]
    float publishMessageFrequency = 0.02f;
    private float timeElapsed;

    private float xVel;
    private float yVel;

    float lastInputTime;

    bool pub_message = false;

    // Start is called before the first frame update
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistCommandMsg>(topicName);

        lastInputTime = Time.time;

    }

    private void OnEnable()
    {
        enableMove[inputSource].onState += MoveArmInPlane;
    }

    private void MoveArmInPlane(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        float xInput = xyInput[inputSource].axis.x;
        float yInput = xyInput[inputSource].axis.y;

        xVel = yInput * maxLinearVel;
        yVel = -xInput * maxLinearVel;

        lastInputTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Delta: " + scroll.delta);
        //Debug.Log("Axis: " + scroll.axis);
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishMessageFrequency)
        {
            if(Time.time - lastInputTime < 0.1f)
            {
                TwistCommandMsg twistCmd = new TwistCommandMsg();
                twistCmd.twist.linear_x = xVel;
                twistCmd.twist.linear_y = yVel;
                ros.Publish(topicName, twistCmd);
                pub_message = true;

            } else if (pub_message)
            {
                ros.Publish(topicName, new TwistCommandMsg());
                pub_message = false;
            } 
        }
    }
}
