//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Control
{
    [Serializable]
    public class GripperCommandResult : Message
    {
        public const string k_RosMessageName = "control_msgs/GripperCommand";
        public override string RosMessageName => k_RosMessageName;

        public double position;
        //  The current gripper gap size (in meters)
        public double effort;
        //  The current effort exerted (in Newtons)
        public bool stalled;
        //  True iff the gripper is exerting max effort and not moving
        public bool reached_goal;
        //  True iff the gripper position has reached the commanded setpoint

        public GripperCommandResult()
        {
            this.position = 0.0;
            this.effort = 0.0;
            this.stalled = false;
            this.reached_goal = false;
        }

        public GripperCommandResult(double position, double effort, bool stalled, bool reached_goal)
        {
            this.position = position;
            this.effort = effort;
            this.stalled = stalled;
            this.reached_goal = reached_goal;
        }

        public static GripperCommandResult Deserialize(MessageDeserializer deserializer) => new GripperCommandResult(deserializer);

        private GripperCommandResult(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.position);
            deserializer.Read(out this.effort);
            deserializer.Read(out this.stalled);
            deserializer.Read(out this.reached_goal);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.position);
            serializer.Write(this.effort);
            serializer.Write(this.stalled);
            serializer.Write(this.reached_goal);
        }

        public override string ToString()
        {
            return "GripperCommandResult: " +
            "\nposition: " + position.ToString() +
            "\neffort: " + effort.ToString() +
            "\nstalled: " + stalled.ToString() +
            "\nreached_goal: " + reached_goal.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Result);
        }
    }
}
