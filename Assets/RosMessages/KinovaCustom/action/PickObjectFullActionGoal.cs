using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class PickObjectFullActionGoal : ActionGoal<PickObjectFullGoal>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PickObjectFullActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public PickObjectFullActionGoal() : base()
        {
            this.goal = new PickObjectFullGoal();
        }

        public PickObjectFullActionGoal(HeaderMsg header, GoalIDMsg goal_id, PickObjectFullGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static PickObjectFullActionGoal Deserialize(MessageDeserializer deserializer) => new PickObjectFullActionGoal(deserializer);

        PickObjectFullActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = PickObjectFullGoal.Deserialize(deserializer);
        }
        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.header);
            serializer.Write(this.goal_id);
            serializer.Write(this.goal);
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
