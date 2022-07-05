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

public class ArmTarget : MonoBehaviour
{

    ROSConnection ros;

    [SerializeField]
    string poseTopic = "/kinova_moveit_pose_goal";

    [SerializeField]
    string cartesianTopic = "/kinova_moveit_cartesian_goal";

    [SerializeField]
    string jointTopic = "/kinova_moveit_joint_goal";

    [SerializeField]
    string stopTopic = "/kinova_arm/in/stop";

    [SerializeField]
    string placeTopic = "/kinova_place_object_server/goal";

    [SerializeField]
    string moveGroupResultTopic = "/move_group/result";

    public enum ArmState
    {
        Default,
        InGraspPosition,
        HomePosition
    }

    public ArmState armState = ArmState.HomePosition;

    
    [SerializeField]
    string[] jointNames = new string[7];

    public GameObject targetLinkObject = null;
    GameObject baseLinkObject = null;
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

        ros.Subscribe<MoveGroupActionResult>(moveGroupResultTopic, MoveGroupResultCallback);

        StartCoroutine(SetBaseLinkObject());

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
        PoseMsg pose = GetTargetPoseMsg();
        ros.Publish(cartesianTopic, pose);
    }

    private PoseMsg GetTargetPoseMsg()
    {
        Vector3 targetPosition = baseLinkObject.transform.InverseTransformPoint(targetLinkObject.transform.position);

        var targetPositionROS = targetPosition.To<FLU>();
        targetPositionROS += staticPositionOffset;
        var targetRotationROS = transform.localRotation.To<FLU>();

        return new PoseMsg(targetPositionROS, targetRotationROS);
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
    }

    public void PublishJointGoal(double[] jointPositions)
    {
        var jointTarget = new JointStateMsg();
        jointTarget.name = jointNames;
        jointTarget.position = jointPositions;
        ros.Publish(jointTopic, jointTarget);
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

    private void MoveGroupResultCallback(MoveGroupActionResult msg)
    {
        if (msg.status.status == 3)
        {
            armState = ArmState.InGraspPosition;
        }
    }


}
