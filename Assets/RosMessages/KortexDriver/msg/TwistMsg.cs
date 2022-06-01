//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.KortexDriver
{
    [Serializable]
    public class TwistMsg : Message
    {
        public const string k_RosMessageName = "kortex_driver/Twist";
        public override string RosMessageName => k_RosMessageName;

        public float linear_x;
        public float linear_y;
        public float linear_z;
        public float angular_x;
        public float angular_y;
        public float angular_z;

        public TwistMsg()
        {
            this.linear_x = 0.0f;
            this.linear_y = 0.0f;
            this.linear_z = 0.0f;
            this.angular_x = 0.0f;
            this.angular_y = 0.0f;
            this.angular_z = 0.0f;
        }

        public TwistMsg(float linear_x, float linear_y, float linear_z, float angular_x, float angular_y, float angular_z)
        {
            this.linear_x = linear_x;
            this.linear_y = linear_y;
            this.linear_z = linear_z;
            this.angular_x = angular_x;
            this.angular_y = angular_y;
            this.angular_z = angular_z;
        }

        public static TwistMsg Deserialize(MessageDeserializer deserializer) => new TwistMsg(deserializer);

        private TwistMsg(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.linear_x);
            deserializer.Read(out this.linear_y);
            deserializer.Read(out this.linear_z);
            deserializer.Read(out this.angular_x);
            deserializer.Read(out this.angular_y);
            deserializer.Read(out this.angular_z);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.linear_x);
            serializer.Write(this.linear_y);
            serializer.Write(this.linear_z);
            serializer.Write(this.angular_x);
            serializer.Write(this.angular_y);
            serializer.Write(this.angular_z);
        }

        public override string ToString()
        {
            return "TwistMsg: " +
            "\nlinear_x: " + linear_x.ToString() +
            "\nlinear_y: " + linear_y.ToString() +
            "\nlinear_z: " + linear_z.ToString() +
            "\nangular_x: " + angular_x.ToString() +
            "\nangular_y: " + angular_y.ToString() +
            "\nangular_z: " + angular_z.ToString();
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
