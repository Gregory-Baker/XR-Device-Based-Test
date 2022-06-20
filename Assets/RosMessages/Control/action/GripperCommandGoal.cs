//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Control
{
    [Serializable]
    public class GripperCommandGoal : Message
    {
        public const string k_RosMessageName = "control_msgs/GripperCommand";
        public override string RosMessageName => k_RosMessageName;

        public GripperCommandMsg command;

        public GripperCommandGoal()
        {
            this.command = new GripperCommandMsg();
        }

        public GripperCommandGoal(GripperCommandMsg command)
        {
            this.command = command;
        }

        public static GripperCommandGoal Deserialize(MessageDeserializer deserializer) => new GripperCommandGoal(deserializer);

        private GripperCommandGoal(MessageDeserializer deserializer)
        {
            this.command = GripperCommandMsg.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.command);
        }

        public override string ToString()
        {
            return "GripperCommandGoal: " +
            "\ncommand: " + command.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Goal);
        }
    }
}
