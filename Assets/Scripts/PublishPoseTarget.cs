using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using System;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class PublishPoseTarget : MonoBehaviour
{

    ROSConnection ros;

    [SerializeField]
    string topicName = "/kinova_moveit_pose_goal";


    public GameObject targetLinkObject = null;

    public GameObject baseLinkObject = null;

    public Vector3<FLU> staticPositionOffset = Vector3<FLU>.zero;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseMsg>(topicName);

        StartCoroutine(SetBaseLinkObject());

    }

    private IEnumerator SetBaseLinkObject()
    {
        while (baseLinkObject == null)
        {
            baseLinkObject = GameObject.Find("base_link");
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
    public void Publish()
    {
        Vector3 targetPosition = baseLinkObject.transform.InverseTransformPoint(targetLinkObject.transform.position);

        var targetPositionROS = targetPosition.To<FLU>();
        targetPositionROS += staticPositionOffset;

        var targetRotationROS = transform.localRotation.To<FLU>();

        Debug.DrawRay(baseLinkObject.transform.position, targetPosition, Color.green);

        Quaternion<FLU> dummyQuaternion = new Quaternion<FLU>(-0.7071f, -0.7071f, 0, 0);

        PoseMsg pose = new PoseMsg(targetPositionROS, targetRotationROS);

        Debug.Log("x: " + targetPositionROS.x);
        Debug.Log("y: " + targetPositionROS.y);
        Debug.Log("z: " + targetPositionROS.z);

        Debug.Log("i: " + targetRotationROS.x);
        Debug.Log("j: " + targetRotationROS.y);
        Debug.Log("k: " + targetRotationROS.z);
        Debug.Log("w: " + targetRotationROS.w);

        ros.Publish(topicName, pose);
    }
}
