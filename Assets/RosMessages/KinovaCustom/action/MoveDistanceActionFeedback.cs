using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class MoveDistanceActionFeedback : ActionFeedback<MoveDistanceFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/MoveDistanceActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public MoveDistanceActionFeedback() : base()
        {
            this.feedback = new MoveDistanceFeedback();
        }

        public MoveDistanceActionFeedback(HeaderMsg header, GoalStatusMsg status, MoveDistanceFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static MoveDistanceActionFeedback Deserialize(MessageDeserializer deserializer) => new MoveDistanceActionFeedback(deserializer);

        MoveDistanceActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = MoveDistanceFeedback.Deserialize(deserializer);
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
