using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.KinovaCustom;
using RosMessageTypes.Actionlib;
using RosMessageTypes.Moveit;
using RosMessageTypes.Control;
using UnityEngine.UIElements;

public class ArmTarget : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    ParticipantHandler participantHandler;

    public enum ArmState
    {
        Default,
        ExecutingAction,
        HomePosition
    }

    public ArmState armState = ArmState.HomePosition;


    [Header("ROS Topics")]
    [SerializeField]
    string poseTopic = "/kinova_moveit_pose_goal";

    [SerializeField]
    string cartesianTopic = "/kinova_moveit_cartesian_goal";

    [SerializeField]
    string jointTopic = "/kinova_moveit_joint_goal";

    [SerializeField]
    string stopTopic = "/kinova_arm/in/stop";

    [SerializeField]
    string pickTopic = "/kinova_moveit_pick_goal";

    [SerializeField]
    string pickResultTopic = "/kinova_moveit_pick_result";

    [SerializeField]
    string cancelPickTopic;

    [SerializeField]
    string placeTopic = "/kinova_place_object_server/goal";

    [SerializeField]
    string cancelPlaceTopic;

    [SerializeField]
    string placeResultTopic = "/kinova_place_object_server/result";

    [SerializeField]
    string executeTrajectoryActionResult = "/move_group/result";

    [SerializeField]
    string gripGoalTopic = "/kinova_arm/kinova_arm_robotiq_2f_85_gripper_controller/gripper_cmd/goal";

    [SerializeField]
    string gripResultTopic = "/kinova_arm/kinova_arm_robotiq_2f_85_gripper_controller/gripper_cmd/result";

    [SerializeField]
    string[] jointNames = new string[7];


    [Header("External Game Objects")]
    public GameObject targetLinkObject = null;
    GameObject baseLinkObject = null;
    GameObject targetFrame = null;

    public ChangeArmTargetControl changeArmTargetControl;

    [Header("Variables")]
    [SerializeField]
    float pickHeight = 0.125f;

    public Vector3<FLU> staticPositionOffset = Vector3<FLU>.zero;

    [Header("Read Only")]
    [SerializeField]
    float delay = 0;
    public bool gripperClosed = false;

    // Start is called before the first frame update
    void Start()
    {

        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(poseTopic);
        ros.RegisterPublisher<PoseMsg>(cartesianTopic);
        ros.RegisterPublisher<JointStateMsg>(jointTopic);
        ros.RegisterPublisher<EmptyMsg>(stopTopic);
        ros.RegisterPublisher<PlaceObjectActionGoal>(placeTopic);
        ros.RegisterPublisher<PlaceObjectActionGoal>(pickTopic);
        ros.RegisterPublisher<GoalIDMsg>(cancelPlaceTopic);
        ros.RegisterPublisher<GoalIDMsg>(cancelPickTopic);
        ros.RegisterPublisher<GripperCommandActionGoal>(gripGoalTopic);

        ros.Subscribe<PlaceObjectActionResult>(pickResultTopic, PickResultCallback);
        ros.Subscribe<PlaceObjectActionResult>(placeResultTopic, PlaceResultCallback);

        StartCoroutine(SetBaseLinkObject());
        StartCoroutine(SetTargetFrameObject());

        if (armState == ArmState.HomePosition)
            ArmToHomePosition();

        OpenGripper();
    }

    private IEnumerator SetTargetFrameObject()
    {
        while (targetFrame == null)
        {
            targetFrame = GameObject.Find("kinova_arm_tool_frame");
            yield return null;
        }
    }

    private IEnumerator SetBaseLinkObject()
    {
        while (baseLinkObject == null)
        {
            baseLinkObject = GameObject.Find("base_link");
            yield return null;
        }
    }

    public void StopArm()
    {
        PublishWithDelay(stopTopic, new EmptyMsg());
        if (armState == ArmState.ExecutingAction)
        {
            GoalIDMsg goalIDMsg = new GoalIDMsg();
            PublishWithDelay(cancelPickTopic, goalIDMsg);
            PublishWithDelay(cancelPlaceTopic, goalIDMsg);
        }
        StartCoroutine(MoveTargetToRobotCoroutine());
    }


    public void PublishPoseGoal()
    {
        PoseMsg pose = GetTargetPoseMsg();
        PublishWithDelay(poseTopic, pose);
    }

    public void PublishJointGoal()
    {
        PoseMsg pose = GetTargetPoseMsg();
        PublishWithDelay(jointTopic, pose);
    }

    public void PublishCartesianGoal()
    {
        armState = ArmState.Default;
        PoseMsg pose = GetTargetPoseMsg();
        PublishWithDelay(cartesianTopic, pose);
    }

    public void CloseGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = 0.8;
        gripperClosed = true;
        PublishWithDelay(gripGoalTopic, gripperCmd);
    }

    public void OpenGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = 0.0;
        gripperClosed = false;
        PublishWithDelay(gripGoalTopic, gripperCmd);
    }

    public void ActuateGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = (gripperClosed) ? 0.0 : 0.8;
        gripperClosed = !gripperClosed;
        PublishWithDelay(gripGoalTopic, gripperCmd);
    }

    private PoseMsg GetTargetPoseMsg()
    {
        Vector3 targetPosition = baseLinkObject.transform.InverseTransformPoint(targetLinkObject.transform.position);

        var targetPositionROS = targetPosition.To<FLU>();
        targetPositionROS += staticPositionOffset;
        var targetRotationROS = transform.localRotation.To<FLU>();

        return new PoseMsg(targetPositionROS, targetRotationROS);
    }

    private Vector3 GetEndEffectorPosition()
    {
        Vector3 eefPosition = baseLinkObject.transform.InverseTransformPoint(targetFrame.transform.position);
        return eefPosition;
    }

    private void MoveTargetToRobot()
    {
        Vector3 eefPosition = GetEndEffectorPosition();
        transform.localPosition = eefPosition - staticPositionOffset.toUnity;
    }
    private IEnumerator MoveTargetToRobotCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 eefPosition = GetEndEffectorPosition();
        transform.localPosition = eefPosition - staticPositionOffset.toUnity;
    }


    public PoseStampedMsg GetTargetPoseStampedMsg()
    {
        PoseStampedMsg poseStampedMsg = new PoseStampedMsg();

        poseStampedMsg.pose = GetTargetPoseMsg();

        return poseStampedMsg;
    }

    public void ArmToHomePosition()
    {
        transform.localPosition = new Vector3(-0.190f, 0.518f, 0.409f);

        var jointPositions = new double[7];
        jointPositions[0] = 1.516;
        jointPositions[1] = -1.177;
        jointPositions[2] = -0.880;
        jointPositions[3] = 2.153;
        jointPositions[4] = -0.812;
        jointPositions[5] = 1.898;
        jointPositions[6] = -0.798;
        PublishJointGoal(jointPositions);
        armState = ArmState.HomePosition;
    }

    public void PublishJointGoal(double[] jointPositions)
    {
        var jointTarget = new JointStateMsg();
        jointTarget.name = jointNames;
        jointTarget.position = jointPositions;
        PublishWithDelay(jointTopic, jointTarget);
    }


    //public void PickObject()
    //{
    //    var pick_msg = new Float32Msg(pickHeight);
    //    PublishWithDelay(pickTopic, pick_msg);
    //}

    public void PickObject()
    {
        if (armState == ArmState.Default)
        {
            armState = ArmState.ExecutingAction;
            var pick_msg = new PlaceObjectActionGoal();
            PublishWithDelay(pickTopic, pick_msg);
            gripperClosed = true;
        }
    }

    private void PickResultCallback(BoolMsg msg)
    {
        changeArmTargetControl.SetControlMode(ChangeArmTargetControl.ControlMode.ConfirmActionSuccess);
    }

    private void PickResultCallback(PlaceObjectActionResult msg)
    {
        armState = ArmState.Default;
        changeArmTargetControl.SetControlMode(ChangeArmTargetControl.ControlMode.ConfirmActionSuccess);
    }


    public void PlaceObject()
    {
        if (armState == ArmState.Default)
        {
            armState = ArmState.ExecutingAction;
            var place_msg = new PlaceObjectActionGoal();
            PublishWithDelay(placeTopic, place_msg);
            gripperClosed = false;
        }
    }

    private void PlaceResultCallback(PlaceObjectActionResult msg)
    {
        armState = ArmState.Default;
        if (msg.status.status == 3)
        {
            changeArmTargetControl.SetControlMode(ChangeArmTargetControl.ControlMode.ConfirmActionSuccess);
        }
    }

    private static void PrintTargetPose(Vector3<FLU> targetPositionROS, Quaternion<FLU> targetRotationROS)
    {
        Debug.Log("Position: " + targetPositionROS);
        Debug.Log("Orientation: " + targetRotationROS);
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
        delay = participantHandler.delay;
    }
}
