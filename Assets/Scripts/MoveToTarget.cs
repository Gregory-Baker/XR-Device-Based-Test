using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using RosMessageTypes.Mbf;

public class MoveToTarget : MonoBehaviour
{
    
    [SerializeField]
    GameObject targetObject = null;

    [SerializeField]
    string goalTopicName = null;

    [SerializeField]
    string cancelGoalTopicName = null;

    [SerializeField]
    string moveBaseActionResultTopicName = null;

    [SerializeField]
    bool publishTarget = true;

    [SerializeField]
    bool enableTurn = true;

    [SerializeField]
    float turnAngle = 90;

    [SerializeField]
    RosHeadRotationPublisher panTiltPublisher;
    
    ROSConnection ros;
    public GameObject robotBaseLink;

    float previousRobotHeading = 0f;

    public enum ActionStatus {
        ABORTED,
        IN_PROGRESS,
        SUCCEEDED
    }

    public ActionStatus moveBaseActionStatus = ActionStatus.SUCCEEDED; 

    private void OnEnable()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(goalTopicName);
        ros.RegisterPublisher<GoalIDMsg>(cancelGoalTopicName);
        ros.Subscribe<MoveBaseActionResult>(moveBaseActionResultTopicName, MoveBaseResultCallback);

        BaseController.OnConfirm += ConfirmTargetPosition;
        BaseController.OnTurnLeft += TurnLeft;
        BaseController.OnTurnRight += TurnRight;
        BaseController.OnStop += StopRobot;

        StartCoroutine(SetBaseLinkObject());
    }

    private IEnumerator SetBaseLinkObject()
    {
        while (robotBaseLink == null)
        {
            robotBaseLink = GameObject.Find("base_link");
            yield return null;
        }
        SetStartPosition();
    }

    private void MoveBaseResultCallback(MoveBaseActionResult msg)
    {
        if (msg.result.outcome == 0)
        {
            moveBaseActionStatus = ActionStatus.SUCCEEDED;
        }
        else
        {
            moveBaseActionStatus = ActionStatus.ABORTED;
        }
    }

    private void OnDisable()
    {
        BaseController.OnConfirm -= ConfirmTargetPosition;
        BaseController.OnTurnLeft -= TurnLeft;
        BaseController.OnTurnRight -= TurnRight;
        BaseController.OnStop -= StopRobot;
    }

    private void SetStartPosition()
    {
        if (robotBaseLink != null)
        {
            targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
            transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);

            previousRobotHeading = robotBaseLink.transform.eulerAngles.y;
        }
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
            moveBaseActionStatus = ActionStatus.IN_PROGRESS;
        }

        if (Mathf.Abs(panTiltPublisher.panOffset) > 0)
        {
            StartCoroutine(HoldCameraStationaryInTurn());
        }
    }

    private IEnumerator HoldCameraStationaryInTurn()
    {
        while (moveBaseActionStatus == ActionStatus.IN_PROGRESS)
        {
            float currentRobotHeading = robotBaseLink.transform.eulerAngles.y;
            float headingChange = currentRobotHeading - previousRobotHeading;
            panTiltPublisher.panOffset -= headingChange;
            previousRobotHeading = currentRobotHeading;
            yield return null;
        }
        panTiltPublisher.panOffset = 0;
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
