using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class PlaceObjectActionFeedback : ActionFeedback<PlaceObjectFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PlaceObjectActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public PlaceObjectActionFeedback() : base()
        {
            this.feedback = new PlaceObjectFeedback();
        }

        public PlaceObjectActionFeedback(HeaderMsg header, GoalStatusMsg status, PlaceObjectFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static PlaceObjectActionFeedback Deserialize(MessageDeserializer deserializer) => new PlaceObjectActionFeedback(deserializer);

        PlaceObjectActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = PlaceObjectFeedback.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.feedback);
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
