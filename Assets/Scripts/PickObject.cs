using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Moveit;
using RosMessageTypes.Trajectory;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;

public class PickObject : MonoBehaviour
{
    ROSConnection ros;

    public ArmTarget armTarget;

    public string pickupTopic = "/pickup/goal";

    private void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PickupActionGoal>(pickupTopic);

        armTarget = GetComponent<ArmTarget>();
    }

    public void PublishPickGoal()
    {
        PickupActionGoal pickupActionGoal = new PickupActionGoal();
        // pickupActionGoal.goal.target_name = "barrel";
        pickupActionGoal.goal.group_name = "manipulator";
        pickupActionGoal.goal.end_effector = "gripper";

        GraspMsg grasp = new GraspMsg();
        HeaderMsg header = new HeaderMsg();
        string[] joint_names = new string[] { "kinova_arm_finger_joint" };
        JointTrajectoryPointMsg[] pre_grasp_trajectory_points = new JointTrajectoryPointMsg[1] { new JointTrajectoryPointMsg()};

        Array.Resize(ref pre_grasp_trajectory_points[0].positions, 1);
        pre_grasp_trajectory_points[0].positions[0] = 0.0;
        JointTrajectoryMsg pre_grasp_posture = new JointTrajectoryMsg(header, joint_names, pre_grasp_trajectory_points);

        grasp.grasp_pose.header.frame_id = "base_link";
        grasp.grasp_pose = armTarget.GetTargetPoseStampedMsg();

        grasp.pre_grasp_posture = pre_grasp_posture;

        grasp.pre_grasp_approach.direction.header.frame_id = "base_link";
        grasp.pre_grasp_approach.direction.vector.z = 1.0f;
        grasp.pre_grasp_approach.min_distance = 0.1f;
        grasp.pre_grasp_approach.desired_distance = 0.2f;

        grasp.post_grasp_retreat.direction.header.frame_id = "base_link";
        grasp.post_grasp_retreat.direction.vector.z = 1.0f;
        grasp.post_grasp_retreat.min_distance = 0.1f;
        grasp.post_grasp_retreat.desired_distance = 0.2f;

        JointTrajectoryPointMsg[] grasp_trajectory_points = new JointTrajectoryPointMsg[1] { new JointTrajectoryPointMsg() };
        Array.Resize(ref grasp_trajectory_points[0].positions, 1);
        grasp_trajectory_points[0].positions[0] = 0.8;
        JointTrajectoryMsg grasp_posture = new JointTrajectoryMsg(header, joint_names, grasp_trajectory_points);

        grasp.grasp_posture = grasp_posture;

        Array.Resize(ref pickupActionGoal.goal.possible_grasps, 1);
        pickupActionGoal.goal.possible_grasps[0] = grasp;

        ros.Publish(pickupTopic, pickupActionGoal);

    } 
}
