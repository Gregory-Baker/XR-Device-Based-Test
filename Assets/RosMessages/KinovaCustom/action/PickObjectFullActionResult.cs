using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class PickObjectFullActionResult : ActionResult<PickObjectFullResult>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PickObjectFullActionResult";
        public override string RosMessageName => k_RosMessageName;


        public PickObjectFullActionResult() : base()
        {
            this.result = new PickObjectFullResult();
        }

        public PickObjectFullActionResult(HeaderMsg header, GoalStatusMsg status, PickObjectFullResult result) : base(header, status)
        {
            this.result = result;
        }
        public static PickObjectFullActionResult Deserialize(MessageDeserializer deserializer) => new PickObjectFullActionResult(deserializer);

        PickObjectFullActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = PickObjectFullResult.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.status);
            serializer.Write(this.result);
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
