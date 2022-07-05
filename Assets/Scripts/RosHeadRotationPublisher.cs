using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.DynamixelPanTilt;
using UnityEngine.XR;
using Valve.VR;
using System.Linq;
using UnityEngine.UIElements;

public class RosHeadRotationPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "head_rot";
    public float publishMessageFrequency = 0f;
    private float timeElapsed;

    public float panOffset = 0;
    public float tiltOffset = 0;

    private PanTiltAngleMsg headPosition;
    private Quaternion headRotation;
    private List<XRNodeState> nodeStates = new List<XRNodeState>();

    float prevPanAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PanTiltAngleMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (ros.isActiveAndEnabled && timeElapsed > publishMessageFrequency)
        {
            InputTracking.GetNodeStates(nodeStates);
            var headState = nodeStates.FirstOrDefault(node => node.nodeType == XRNode.Head);
            headState.TryGetRotation(out headRotation);
            Vector3 angles = headRotation.eulerAngles;
            float panAngle = angles.y + panOffset;
            float tiltAngle = angles.x + tiltOffset;
            if (panAngle > 180)
            {
                panAngle -= 360;
            }
            if (tiltAngle > 180)
            {
                tiltAngle -= 360;
            }
            float panChange = Mathf.Abs(panAngle - prevPanAngle);
            if (panChange > 270)
            {
                panAngle = prevPanAngle;
            }
            PanTiltAngleMsg headRotMsg = new PanTiltAngleMsg(panAngle, tiltAngle);
            ros.Publish(topicName, headRotMsg);
            prevPanAngle = panAngle;
            timeElapsed = 0;
        }
    }

    public void CentreCamera()
    {
        panOffset = 0;
        tiltOffset = 0;
    }
}
