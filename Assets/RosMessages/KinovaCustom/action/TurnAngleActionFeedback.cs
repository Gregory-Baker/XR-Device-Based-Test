using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class TurnAngleActionFeedback : ActionFeedback<TurnAngleFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/TurnAngleActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public TurnAngleActionFeedback() : base()
        {
            this.feedback = new TurnAngleFeedback();
        }

        public TurnAngleActionFeedback(HeaderMsg header, GoalStatusMsg status, TurnAngleFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static TurnAngleActionFeedback Deserialize(MessageDeserializer deserializer) => new TurnAngleActionFeedback(deserializer);

        TurnAngleActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = TurnAngleFeedback.Deserialize(deserializer);
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
