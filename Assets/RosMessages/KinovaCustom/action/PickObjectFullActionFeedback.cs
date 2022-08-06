using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class PickObjectFullActionFeedback : ActionFeedback<PickObjectFullFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PickObjectFullActionFeedback";
        public override string RosMessageName => k_RosMessageName;


        public PickObjectFullActionFeedback() : base()
        {
            this.feedback = new PickObjectFullFeedback();
        }

        public PickObjectFullActionFeedback(HeaderMsg header, GoalStatusMsg status, PickObjectFullFeedback feedback) : base(header, status)
        {
            this.feedback = feedback;
        }
        public static PickObjectFullActionFeedback Deserialize(MessageDeserializer deserializer) => new PickObjectFullActionFeedback(deserializer);

        PickObjectFullActionFeedback(MessageDeserializer deserializer) : base(deserializer)
        {
            this.feedback = PickObjectFullFeedback.Deserialize(deserializer);
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
