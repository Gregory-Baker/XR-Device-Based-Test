using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Control;

public class ActuateGripper : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string topicName = "/kinova_arm/kinova_arm_robotiq_2f_85_gripper_controller/gripper_cmd/goal";

    public bool gripperClosed = false;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<GripperCommandActionGoal>(topicName);
    }

    public void PublishActionGoal()
    {
        var gripperCmd = new GripperCommandActionGoal();

        gripperCmd.goal.command.position = (gripperClosed) ? 0.0 : 0.75;

        gripperClosed = !gripperClosed;

        Debug.Log("Here");

        ros.Publish(topicName, gripperCmd);
    }
}
