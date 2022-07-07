using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class MoveDistanceActionResult : ActionResult<MoveDistanceResult>
    {
        public const string k_RosMessageName = "kinova_custom_actions/MoveDistanceActionResult";
        public override string RosMessageName => k_RosMessageName;


        public MoveDistanceActionResult() : base()
        {
            this.result = new MoveDistanceResult();
        }

        public MoveDistanceActionResult(HeaderMsg header, GoalStatusMsg status, MoveDistanceResult result) : base(header, status)
        {
            this.result = result;
        }
        public static MoveDistanceActionResult Deserialize(MessageDeserializer deserializer) => new MoveDistanceActionResult(deserializer);

        MoveDistanceActionResult(MessageDeserializer deserializer) : base(deserializer)
        {
            this.result = MoveDistanceResult.Deserialize(deserializer);
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
