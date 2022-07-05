using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector;
using System.Linq;
using RosMessageTypes.Actionlib;
// using RosMessageTypes.Mbf;
using RosMessageTypes.Nav;
using RosMessageTypes.MoveBase;

public class MoveToTarget : MonoBehaviour
{
    
    [SerializeField]
    GameObject targetObject = null;

    [SerializeField]
    string cmdVelTopic = null;

    [SerializeField]
    string goalTopicName = null;

    [SerializeField]
    string cancelGoalTopicName = null;

    [SerializeField]
    string moveBaseActionStatusTopic = null;

    [SerializeField]
    bool publishTarget = true;

    [SerializeField]
    float cameraTurnSpeed = 20f;

    [SerializeField]
    float robotTurnSpeed = 20f;

    [SerializeField]
    float moveTargetSpeed = 0.2f;

    [SerializeField]
    RosHeadRotationPublisher panTiltPublisher;
    
    ROSConnection ros;
    public GameObject robotBaseLink;

    float previousRobotHeading = 0f;

    public enum ActionStatus
    {
        FAILED,
        IN_PROGRESS,
        SUCCEEDED
    }

    public ActionStatus moveBaseActionStatus = ActionStatus.SUCCEEDED; 

    private void OnEnable()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(goalTopicName);
        ros.RegisterPublisher<GoalIDMsg>(cancelGoalTopicName);
        ros.RegisterPublisher<TwistMsg>(cmdVelTopic);
        ros.Subscribe<GoalStatusArrayMsg>(moveBaseActionStatusTopic, MoveBaseResultCallback);

        BaseController.OnConfirm += ConfirmTargetPosition;
        BaseController.OnStop += StopRobot;
        BaseController.OnTurnRobotToCam += TurnRobotToCam;
        BaseController.OnTurnCamLeft += TurnCamLeft;
        BaseController.OnTurnCamRight += TurnCamRight;
        BaseController.OnMoveForward += MoveTargetForward;
        BaseController.OnMoveBackward += MoveTargetBackward;
        BaseController.OnMoveRobotToTarget += MoveRobotToTarget;
        BaseController.OnMoveTargetToRobot += MoveTargetToRobot;


        StartCoroutine(SetBaseLinkObject());
    }

    private void OnDisable()
    {
        BaseController.OnConfirm -= ConfirmTargetPosition;
        BaseController.OnStop -= StopRobot;
        BaseController.OnTurnRobotToCam -= TurnRobotToCam;
        BaseController.OnTurnCamLeft -= TurnCamLeft;
        BaseController.OnTurnCamRight -= TurnCamRight;
        BaseController.OnMoveForward -= MoveTargetForward;
        BaseController.OnMoveBackward -= MoveTargetBackward;
        BaseController.OnMoveRobotToTarget -= MoveRobotToTarget;
        BaseController.OnMoveTargetToRobot -= MoveTargetToRobot;
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

    private void MoveBaseResultCallback(GoalStatusArrayMsg msg)
    {
        if (msg.status_list.Length > 0)
        {
            var status = msg.status_list.Last().status;
            if (status == 3)
            {
                moveBaseActionStatus = ActionStatus.SUCCEEDED;
            }
            else if (status == 1)
            {
                moveBaseActionStatus = ActionStatus.IN_PROGRESS;
            }
            else
            {
                moveBaseActionStatus = ActionStatus.FAILED;
            }
        }
    }


    private void TurnRobotToCam()
    {
        MoveConfirmToTarget();
        StartCoroutine(TurnRobotToCamCoroutine());
    }

    private void TurnCamLeft()
    {
        if (moveBaseActionStatus != ActionStatus.IN_PROGRESS && panTiltPublisher.panOffset > -179)
        {
            panTiltPublisher.panOffset -= cameraTurnSpeed * Time.deltaTime;
            targetObject.transform.Rotate(Vector3.up, -cameraTurnSpeed * Time.deltaTime);
        }
    }

    private void TurnCamRight()
    {
        if (moveBaseActionStatus != ActionStatus.IN_PROGRESS && panTiltPublisher.panOffset < 179)
        {
            panTiltPublisher.panOffset += cameraTurnSpeed * Time.deltaTime;
            targetObject.transform.Rotate(Vector3.up, cameraTurnSpeed * Time.deltaTime);
        }
    }

    private IEnumerator TurnRobotToCamCoroutine()
    {
        // Degrees per second
        TwistMsg twistMsg = new TwistMsg();
        twistMsg.angular.z = Mathf.Deg2Rad * robotTurnSpeed * -Mathf.Sign(panTiltPublisher.panOffset);
        while (Mathf.Abs(panTiltPublisher.panOffset) > 2)
        {
            // Debug.Log(panTiltPublisher.panOffset); 
            float currentRobotHeading = robotBaseLink.transform.eulerAngles.y;
            float headingChange = currentRobotHeading - previousRobotHeading;
            if (headingChange > 180) headingChange -= 360;
            if (headingChange < -180) headingChange += 360;
            panTiltPublisher.panOffset -= headingChange;
            previousRobotHeading = currentRobotHeading;
            ros.Publish(cmdVelTopic, twistMsg);
            yield return null;
        }
        twistMsg = new TwistMsg();
        ros.Publish(cmdVelTopic, twistMsg);
        panTiltPublisher.CentreCamera();
        MoveTargetToRobot();
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
        panTiltPublisher.CentreCamera();
    }

    private void StopRobot()
    {
        GoalIDMsg cancelGoal = new GoalIDMsg();
        ros.Publish(cancelGoalTopicName, cancelGoal);
        MoveTargetToRobot();
        PublishTarget();
        panTiltPublisher.CentreCamera();
    }

    private void MoveConfirmToTarget()
    {
        transform.SetPositionAndRotation(targetObject.transform.position, targetObject.transform.rotation);
    }

    private void MoveTargetToRobot()
    {
        targetObject.transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
        transform.SetPositionAndRotation(robotBaseLink.transform.position, robotBaseLink.transform.rotation);
    }

    private void MoveTargetBackward()
    {
        Vector3 translation = new Vector3(0, 0, -moveTargetSpeed * Time.deltaTime);
        targetObject.transform.Translate(translation, Space.Self);
    }

    private void MoveTargetForward()
    {
        Vector3 translation = new Vector3(0, 0, moveTargetSpeed * Time.deltaTime);
        targetObject.transform.Translate(translation, Space.Self);
    }

    private void MoveRobotToTarget(int direction)
    {
        MoveConfirmToTarget();
        StartCoroutine(MoveRobotToTargetCoroutine(direction));
    }

    private IEnumerator MoveRobotToTargetCoroutine(int direction)
    {
        float distance = Vector3.Distance(targetObject.transform.position, robotBaseLink.transform.position);
        Vector3Msg linear = new Vector3Msg(direction * moveTargetSpeed, 0, 0);
        Vector3Msg angular = new Vector3Msg();
        TwistMsg twist = new TwistMsg(linear, angular);
        while (distance > 0.05)
        {
            ros.Publish(cmdVelTopic, twist);
            distance = Vector3.Distance(targetObject.transform.position, robotBaseLink.transform.position);
            yield return null;
        }
        ros.Publish(cmdVelTopic, new TwistMsg());
    }


}
