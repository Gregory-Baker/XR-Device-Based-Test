//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.KinovaCustom
{
    [Serializable]
    public class PlaceObjectFeedback : Message
    {
        public const string k_RosMessageName = "kinova_custom_actions/PlaceObject";
        public override string RosMessageName => k_RosMessageName;

        // feedback
        public float wrench_force_z;

        public PlaceObjectFeedback()
        {
            this.wrench_force_z = 0.0f;
        }

        public PlaceObjectFeedback(float wrench_force_z)
        {
            this.wrench_force_z = wrench_force_z;
        }

        public static PlaceObjectFeedback Deserialize(MessageDeserializer deserializer) => new PlaceObjectFeedback(deserializer);

        private PlaceObjectFeedback(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.wrench_force_z);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.wrench_force_z);
        }

        public override string ToString()
        {
            return "PlaceObjectFeedback: " +
            "\nwrench_force_z: " + wrench_force_z.ToString();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize, MessageSubtopic.Feedback);
        }
    }
}
