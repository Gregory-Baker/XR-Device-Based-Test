//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.KinovaCustom
{
    [Serializable]
    public class PlaceObjectResult : Message
    {
        public const string k_RosMessageName = "kinova_custom_actions/PlaceObject";
        public override string RosMessageName => k_RosMessageName;

        // result
        public bool success;

        public PlaceObjectResult()
        {
            this.success = false;
        }

        public PlaceObjectResult(bool success)
        {
            this.success = success;
        }

        public static PlaceObjectResult Deserialize(MessageDeserializer deserializer) => new PlaceObjectResult(deserializer);

        private PlaceObjectResult(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
        }

        public override string ToString()
        {
            return "PlaceObjectResult: " +
            "\nsuccess: " + success.ToString();
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
