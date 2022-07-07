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
using RosMessageTypes.Moveit;
using RosMessageTypes.Control;

public class ArmTarget : MonoBehaviour
{

    ROSConnection ros;

    public enum ArmState
    {
        Default,
        Pick,
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
    string placeTopic = "/kinova_place_object_server/goal";

    [SerializeField]
    string executeTrajectoryActionResult = "/move_group/result";

    [SerializeField]
    string gripGoalTopic = "/kinova_arm/kinova_arm_robotiq_2f_85_gripper_controller/gripper_cmd/goal";

    [SerializeField]
    string gripResultTopic = "/kinova_arm/kinova_arm_robotiq_2f_85_gripper_controller/gripper_cmd/result";

    public bool gripperClosed = false;

    [Header("Variables")]
    [SerializeField]
    float pickHeight = 0.125f;

    
    [SerializeField]
    string[] jointNames = new string[7];

    public GameObject targetLinkObject = null;
    GameObject baseLinkObject = null;
    GameObject targetFrame = null;
    public Vector3<FLU> staticPositionOffset = Vector3<FLU>.zero;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(poseTopic);
        ros.RegisterPublisher<PoseMsg>(cartesianTopic);
        ros.RegisterPublisher<JointStateMsg>(jointTopic);
        ros.RegisterPublisher<EmptyMsg>(stopTopic);
        ros.RegisterPublisher<PlaceObjectActionGoal>(placeTopic);
        ros.RegisterPublisher<Float32Msg>(pickTopic);
        ros.RegisterPublisher<GripperCommandActionGoal>(gripGoalTopic);

        ros.Subscribe<ExecuteTrajectoryActionResult>(executeTrajectoryActionResult, executeTrajectoryResultCallback);
        ros.Subscribe<GripperCommandActionResult>(gripResultTopic, gripResultCallback);

        StartCoroutine(SetBaseLinkObject());
        StartCoroutine(SetTargetFrameObject());

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
        ArmToHomePosition();
    }

    public void StopArm()
    {
        ros.Publish(stopTopic, new EmptyMsg());
        StartCoroutine(MoveTargetToRobotCoroutine());
    }


    public void PublishPoseGoal()
    {
        PoseMsg pose = GetTargetPoseMsg();
        ros.Publish(poseTopic, pose);
    }

    public void PublishJointGoal()
    {
        PoseMsg pose = GetTargetPoseMsg();
        ros.Publish(jointTopic, pose);
    }

    public void PublishCartesianGoal()
    {
        armState = ArmState.Default;
        PoseMsg pose = GetTargetPoseMsg();
        ros.Publish(cartesianTopic, pose);
    }

    public void CloseGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = 0.8;
        gripperClosed = true;
        ros.Publish(gripGoalTopic, gripperCmd);
    }

    public void OpenGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = 0.0;
        gripperClosed = false;
        ros.Publish(gripGoalTopic, gripperCmd);
    }

    public void ActuateGripper()
    {
        var gripperCmd = new GripperCommandActionGoal();
        gripperCmd.goal.command.position = (gripperClosed) ? 0.0 : 0.8;
        gripperClosed = !gripperClosed;
        ros.Publish(gripGoalTopic, gripperCmd);
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
        transform.localPosition = new Vector3(-0.130f, 0.518f, 0.409f);

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
        ros.Publish(jointTopic, jointTarget);
    }

    public void GoToPickPoint()
    {
        if (armState == ArmState.Default)
        {
            armState = ArmState.Pick;
            OpenGripper();
            var pick_msg = new Float32Msg(-pickHeight);
            ros.Publish(pickTopic, pick_msg);
        }
    }

    // TODO: Make robust to changes in target position
    public void GoToPostGraspPoint()
    {
        var pick_msg = new Float32Msg(pickHeight);
        ros.Publish(pickTopic, pick_msg);
        armState = ArmState.Default;
    }

    public void PlaceObject()
    {
        var place_msg = new PlaceObjectActionGoal();

        ros.Publish(placeTopic, place_msg);
    }

    private static void PrintTargetPose(Vector3<FLU> targetPositionROS, Quaternion<FLU> targetRotationROS)
    {
        Debug.Log("Position: " + targetPositionROS);
        Debug.Log("Orientation: " + targetRotationROS);
    }

    private void executeTrajectoryResultCallback(ExecuteTrajectoryActionResult msg)
    {
        if (armState == ArmState.Pick && msg.status.status == 3)
        {

            CloseGripper();
            
        }

    }

    private void gripResultCallback(GripperCommandActionResult obj)
    {
        if (armState == ArmState.Pick && gripperClosed)
        {
            GoToPostGraspPoint();
        }

    }
}
