using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;

public class MoveToTarget : MonoBehaviour
{
    
    [SerializeField]
    GameObject targetObject = null;

    [SerializeField]
    string goalTopicName = null;

    [SerializeField]
    string cancelGoalTopicName = null;

    [SerializeField]
    bool publishTarget = true;

    [SerializeField]
    bool enableTurn = true;

    [SerializeField]
    float turnAngle = 90;

    [SerializeField]
    string objectName = "base_link";

    [SerializeField]
    ZEDManager zed;
    
    ROSConnection ros;
    GameObject robotBaseLink;

    private void OnEnable()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(goalTopicName);
        ros.RegisterPublisher<GoalIDMsg>(cancelGoalTopicName);

        BaseController.OnConfirm += ConfirmTargetPosition;
        BaseController.OnTurnLeft += TurnLeft;
        BaseController.OnTurnRight += TurnRight;
        BaseController.OnStop += StopRobot;

        zed.OnZEDReady += SetStartPosition;
    }

    private void Start()
    {
        // SetStartPosition();
    }

    private void OnDisable()
    {
        BaseController.OnConfirm -= ConfirmTargetPosition;
        BaseController.OnTurnLeft -= TurnLeft;
        BaseController.OnTurnRight -= TurnRight;
        BaseController.OnStop -= StopRobot;

        zed.OnZEDReady -= SetStartPosition;
    }

    private void SetStartPosition()
    {
        if (robotBaseLink == null)
        {
            robotBaseLink = GameObject.Find(objectName);
        }

        targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
        transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
    }

    private void ConfirmTargetPosition()
    {
        transform.SetPositionAndRotation(targetObject.transform.position, targetObject.transform.rotation);

        PublishTarget();
    }

    private void PublishTarget()
    {
        if (publishTarget)
        {
            PoseStampedMsg targetPosition = new PoseStampedMsg();
            targetPosition.header = new RosMessageTypes.Std.HeaderMsg();
            targetPosition.header.frame_id = "map";
            targetPosition.pose.position = targetObject.transform.position.To<FLU>();
            targetPosition.pose.orientation = targetObject.transform.rotation.To<FLU>();
            ros.Publish(goalTopicName, targetPosition);
        }
    }

    private void TurnLeft()
    {
        if (enableTurn)
        {
            targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
            targetObject.transform.Rotate(targetObject.transform.up, -turnAngle);
            transform.SetPositionAndRotation(targetObject.transform.position, targetObject.transform.rotation);
            PublishTarget();
        }

    }

    private void TurnRight()
    {
        if (enableTurn)
        {
            targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
            targetObject.transform.Rotate(targetObject.transform.up, turnAngle);
            transform.SetPositionAndRotation(targetObject.transform.position, targetObject.transform.rotation);
            PublishTarget();
        }
    }

    private void StopRobot()
    {
        GoalIDMsg cancelGoal = new GoalIDMsg();
        ros.Publish(cancelGoalTopicName, cancelGoal);
        MoveToBaseLink();
        // PublishTarget();
    }

    private void MoveToBaseLink()
    {
        targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
        transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
    }
}
