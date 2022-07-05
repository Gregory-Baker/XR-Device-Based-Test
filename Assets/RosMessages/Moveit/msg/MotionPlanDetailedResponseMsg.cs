//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Moveit
{
    [Serializable]
    public class MotionPlanDetailedResponseMsg : Message
    {
        public const string k_RosMessageName = "moveit_msgs/MotionPlanDetailedResponse";
        public override string RosMessageName => k_RosMessageName;

        //  The representation of a solution to a planning problem, including intermediate data
        //  The starting state considered for the robot solution path
        public RobotStateMsg trajectory_start;
        //  The group used for planning (usually the same as in the request)
        public string group_name;
        //  Multiple solution paths are reported, each reflecting intermediate steps in the trajectory processing
        //  The list of reported trajectories
        public RobotTrajectoryMsg[] trajectory;
        //  Description of the reported trajectories (name of processing step)
        public string[] description;
        //  The amount of time spent computing a particular step in motion plan computation
        public double[] processing_time;
        //  Status at the end of this plan
        public MoveItErrorCodesMsg error_code;

        public MotionPlanDetailedResponseMsg()
        {
            this.trajectory_start = new RobotStateMsg();
            this.group_name = "";
            this.trajectory = new RobotTrajectoryMsg[0];
            this.description = new string[0];
            this.processing_time = new double[0];
            this.error_code = new MoveItErrorCodesMsg();
        }

        public MotionPlanDetailedResponseMsg(RobotStateMsg trajectory_start, string group_name, RobotTrajectoryMsg[] trajectory, string[] description, double[] processing_time, MoveItErrorCodesMsg error_code)
        {
            this.trajectory_start = trajectory_start;
            this.group_name = group_name;
            this.trajectory = trajectory;
            this.description = description;
            this.processing_time = processing_time;
            this.error_code = error_code;
        }

        public static MotionPlanDetailedResponseMsg Deserialize(MessageDeserializer deserializer) => new MotionPlanDetailedResponseMsg(deserializer);

        private MotionPlanDetailedResponseMsg(MessageDeserializer deserializer)
        {
            this.trajectory_start = RobotStateMsg.Deserialize(deserializer);
            deserializer.Read(out this.group_name);
            deserializer.Read(out this.trajectory, RobotTrajectoryMsg.Deserialize, deserializer.ReadLength());
            deserializer.Read(out this.description, deserializer.ReadLength());
            deserializer.Read(out this.processing_time, sizeof(double), deserializer.ReadLength());
            this.error_code = MoveItErrorCodesMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.trajectory_start);
            serializer.Write(this.group_name);
            serializer.WriteLength(this.trajectory);
            serializer.Write(this.trajectory);
            serializer.WriteLength(this.description);
            serializer.Write(this.description);
            serializer.WriteLength(this.processing_time);
            serializer.Write(this.processing_time);
            serializer.Write(this.error_code);
        }

        public override string ToString()
        {
            return "MotionPlanDetailedResponseMsg: " +
            "\ntrajectory_start: " + trajectory_start.ToString() +
            "\ngroup_name: " + group_name.ToString() +
            "\ntrajectory: " + System.String.Join(", ", trajectory.ToList()) +
            "\ndescription: " + System.String.Join(", ", description.ToList()) +
            "\nprocessing_time: " + System.String.Join(", ", processing_time.ToList()) +
            "\nerror_code: " + error_code.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}