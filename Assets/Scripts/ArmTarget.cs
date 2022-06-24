using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Sensor;

public class ArmTarget : MonoBehaviour
{

    ROSConnection ros;

    [SerializeField]
    string poseTopicName = "/kinova_moveit_pose_goal";

    [SerializeField]
    string jointTopicName = "/kinova_moveit_joint_goal";


    [SerializeField]
    string[] jointNames = new string[7];

    public GameObject targetLinkObject = null;
    public GameObject baseLinkObject = null;
    public Vector3<FLU> staticPositionOffset = Vector3<FLU>.zero;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(poseTopicName);
        ros.RegisterPublisher<JointStateMsg>(jointTopicName);

        StartCoroutine(SetBaseLinkObject());

    }

    private IEnumerator SetBaseLinkObject()
    {
        while (baseLinkObject == null)
        {
            baseLinkObject = GameObject.Find("base_link");
            yield return new WaitForSeconds(0.2f);
        }
        ArmToHomePosition();
    }

    
    public void PublishPoseGoal()
    {
        Vector3 targetPosition = baseLinkObject.transform.InverseTransformPoint(targetLinkObject.transform.position);

        var targetPositionROS = targetPosition.To<FLU>();
        targetPositionROS += staticPositionOffset;
        var targetRotationROS = transform.localRotation.To<FLU>();

        PoseMsg pose = new PoseMsg(targetPositionROS, targetRotationROS);

        // PrintTargetPose(targetPositionROS, targetRotationROS);
        ros.Publish(poseTopicName, pose);
    }

    public void ArmToHomePosition()
    {
        var jointPositions = new double[7];
        jointPositions[0] = Mathf.PI / 2;
        jointPositions[1] = -1.17;
        jointPositions[2] = -0.93;
        jointPositions[3] = 2.27;
        jointPositions[4] = -0.82;
        jointPositions[5] = 1.78;
        jointPositions[6] = -0.68;
        PublishJointGoal(jointPositions);
    }

    public void PublishJointGoal(double[] jointPositions)
    {
        var jointTarget = new JointStateMsg();
        jointTarget.name = jointNames;
        jointTarget.position = jointPositions;
        ros.Publish(jointTopicName, jointTarget);
    }

    private static void PrintTargetPose(Vector3<FLU> targetPositionROS, Quaternion<FLU> targetRotationROS)
    {
        Debug.Log("Position: " + targetPositionROS);
        Debug.Log("Orientation: " + targetRotationROS);
    }


}
