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
using RosMessageTypes.KinovaCustom;

public class MoveToTarget : MonoBehaviour
{    
    [SerializeField]
    GameObject targetObject = null;

    [SerializeField]
    string cmdVelTopic = null;

    [SerializeField]
    string goalTopicName = null;

    [SerializeField]
    string[] cancelGoalTopicNames = null;

    [SerializeField]
    string moveBaseActionStatusTopic = null;

    [SerializeField]
    string moveBaseActionResultTopic = null;

    [SerializeField]
    string turnRobotActionTopic = null;

    [SerializeField]
    string turnRobotActionResult = null;

    [SerializeField]
    string moveDistanceActionTopic = null;

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

    public bool fixCameraOrientation = false;

    [Header("Read Only")]
    [SerializeField]
    float delay = 0;

    ROSConnection ros;
    public GameObject robotBaseLink;

    float previousRobotHeading = 0f;
    bool turnActionComplete = false;

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
        foreach (var topic in cancelGoalTopicNames)
            ros.RegisterPublisher<GoalIDMsg>(topic);

        ros.RegisterPublisher<TwistMsg>(cmdVelTopic);
        ros.Subscribe<GoalStatusArrayMsg>(moveBaseActionStatusTopic, MoveBaseStatusCallback);
        ros.Subscribe<MoveBaseActionResult>(moveBaseActionResultTopic, MoveBaseActionResultCallback);
        ros.RegisterPublisher<TurnAngleActionGoal>(turnRobotActionTopic);
        ros.Subscribe<TurnAngleActionResult>(turnRobotActionResult, turnRobotActionResultCallback);
        ros.RegisterPublisher<MoveDistanceActionGoal>(moveDistanceActionTopic);

        BaseController.OnConfirm += ConfirmTargetPosition;
        BaseController.OnStop += StopRobot;
        BaseController.OnTurnRobotToCam += TurnRobotToCam;
        BaseController.OnTurnCamLeft += TurnCamLeft;
        BaseController.OnTurnCamRight += TurnCamRight;
        BaseController.OnMoveForward += MoveTargetForward;
        BaseController.OnMoveBackward += MoveTargetBackward;
        BaseController.OnMoveRobotToTarget += MoveRobotToTarget;
        BaseController.OnMoveTargetToRobot += MoveTargetToRobot;
        BaseController.OnTiltCamDown += TiltCamDown;
        BaseController.OnTiltCamUp += TiltCamUp;


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

    void Start()
    {
        delay = FindObjectOfType<ParticipantHandler>().delay;
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

    private void MoveBaseStatusCallback(GoalStatusArrayMsg msg)
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
        // StartCoroutine(TurnRobotToCamCoroutine());
        StartCoroutine(TurnCamWithRobotAction());
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

    private void TiltCamUp()
    {
        if (panTiltPublisher.tiltOffset > -90)
        {
            panTiltPublisher.tiltOffset -= cameraTurnSpeed * Time.deltaTime;
        }
    }
    private void TiltCamDown()
    {
        if (panTiltPublisher.tiltOffset < 90)
        {
            panTiltPublisher.tiltOffset += cameraTurnSpeed * Time.deltaTime;
        }
    }


    private IEnumerator TurnRobotToCamCoroutine()
    {
        // Degrees per second
        TwistMsg twistMsg = new TwistMsg();
        twistMsg.angular.z = Mathf.Deg2Rad * robotTurnSpeed * -Mathf.Sign(panTiltPublisher.panOffset);
        previousRobotHeading = robotBaseLink.transform.eulerAngles.y;
        while (Mathf.Abs(panTiltPublisher.panOffset) > 2)
        {
            // Debug.Log(panTiltPublisher.panOffset); 
            float currentRobotHeading = robotBaseLink.transform.eulerAngles.y;
            float headingChange = currentRobotHeading - previousRobotHeading;
            if (headingChange > 180) headingChange -= 360;
            if (headingChange < -180) headingChange += 360;
            panTiltPublisher.panOffset -= headingChange;
            previousRobotHeading = currentRobotHeading;
            PublishWithDelay(cmdVelTopic, twistMsg);
            yield return null;
        }
        twistMsg = new TwistMsg();
        PublishWithDelay(cmdVelTopic, twistMsg);
        CentreCamera();
        MoveTargetToRobot();
    }

    private IEnumerator TurnCamWithRobotAction()
    {
        TurnAngleGoal turnAngleGoal = new TurnAngleGoal(); 
        turnAngleGoal.turn_angle = - panTiltPublisher.panOffset * Mathf.Deg2Rad;
        TurnAngleActionGoal turnActionGoal = new TurnAngleActionGoal();
        turnActionGoal.goal = turnAngleGoal;
        PublishWithDelay(turnRobotActionTopic, turnActionGoal);

        previousRobotHeading = robotBaseLink.transform.eulerAngles.y;
        while (fixCameraOrientation || !turnActionComplete)
        {
            float currentRobotHeading = robotBaseLink.transform.eulerAngles.y;
            float headingChange = currentRobotHeading - previousRobotHeading;
            if (headingChange > 180) headingChange -= 360;
            if (headingChange < -180) headingChange += 360;
            panTiltPublisher.panOffset -= headingChange;
            previousRobotHeading = currentRobotHeading;
            yield return null;
        }
        turnActionComplete = false;
        CentreCamera();

        MoveTargetToRobot();
    }

    private void CentreCamera()
    {
        if (fixCameraOrientation)
        {
            StartCoroutine(panTiltPublisher.CentreCameraCoroutine(cameraTurnSpeed));
        }
        else
        {
            panTiltPublisher.CentreCamera();
        }
    }

    private void turnRobotActionResultCallback(TurnAngleActionResult msg)
    {
        turnActionComplete = true;
    }

    private void MoveBaseActionResultCallback(MoveBaseActionResult msg)
    {
        CentreCamera();
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
            PublishWithDelay(goalTopicName, targetPosition);
            moveBaseActionStatus = ActionStatus.IN_PROGRESS;
        }

        //if (Mathf.Abs(panTiltPublisher.panOffset) > 0)
        //{
        //    StartCoroutine(HoldCameraStationaryInTurn());
        //}
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
        CentreCamera();
    }

    private void StopRobot()
    {
        GoalIDMsg cancelGoal = new GoalIDMsg();
        foreach (var topic in cancelGoalTopicNames)
            PublishWithDelay(topic, cancelGoal);
        MoveTargetToRobot();
        PublishTarget();
        CentreCamera();
    }

    private void MoveConfirmToTarget()
    {
        transform.SetPositionAndRotation(targetObject.transform.position, targetObject.transform.rotation);
    }

    public void MoveTargetToRobot()
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
        // StartCoroutine(MoveRobotToTargetCoroutine(direction));
        // float targetDistance = Vector3.Distance(transform.position, robotBaseLink.transform.position);
        float targetDistance = robotBaseLink.transform.InverseTransformPoint(transform.position).z;
        MoveRobotToTargetAction(targetDistance);
    }

    private void MoveRobotToTargetAction(float distance)
    {
        MoveDistanceGoal moveDistanceGoal = new MoveDistanceGoal(distance);
        MoveDistanceActionGoal moveDistanceActionGoal = new MoveDistanceActionGoal();
        moveDistanceActionGoal.goal = moveDistanceGoal;

        PublishWithDelay(moveDistanceActionTopic, moveDistanceActionGoal);
    }

    private IEnumerator MoveRobotToTargetCoroutine(int direction)
    {
        float distance = Vector3.Distance(targetObject.transform.position, robotBaseLink.transform.position);
        Vector3Msg linear = new Vector3Msg(direction * moveTargetSpeed, 0, 0);
        Vector3Msg angular = new Vector3Msg();
        TwistMsg twist = new TwistMsg(linear, angular);
        while (distance > 0.05)
        {
            PublishWithDelay(cmdVelTopic, twist);
            distance = Vector3.Distance(targetObject.transform.position, robotBaseLink.transform.position);
            yield return null;
        }
        PublishWithDelay(cmdVelTopic, new TwistMsg());
    }

    public void PublishWithDelay(string topic, Unity.Robotics.ROSTCPConnector.MessageGeneration.Message message)
    {
        StartCoroutine(PublishWithDelayCoroutine(topic, message));
    }

    private IEnumerator PublishWithDelayCoroutine(string topic, Unity.Robotics.ROSTCPConnector.MessageGeneration.Message message)
    {
        yield return new WaitForSeconds(delay);
        ros.Publish(topic, message);
    }

    private void Update()
    {
        if (robotBaseLink!= null && fixCameraOrientation)
        {
            float currentRobotHeading = robotBaseLink.transform.eulerAngles.y;
            float headingChange = currentRobotHeading - previousRobotHeading;
            if (headingChange > 180) headingChange -= 360;
            if (headingChange < -180) headingChange += 360;
            panTiltPublisher.panOffset -= headingChange;
            previousRobotHeading = currentRobotHeading;
        }
    }
}
