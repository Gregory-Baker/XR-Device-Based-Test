using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class MoveDistanceActionGoal : ActionGoal<MoveDistanceGoal>
    {
        public const string k_RosMessageName = "kinova_custom_actions/MoveDistanceActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public MoveDistanceActionGoal() : base()
        {
            this.goal = new MoveDistanceGoal();
        }

        public MoveDistanceActionGoal(HeaderMsg header, GoalIDMsg goal_id, MoveDistanceGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static MoveDistanceActionGoal Deserialize(MessageDeserializer deserializer) => new MoveDistanceActionGoal(deserializer);

        MoveDistanceActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = MoveDistanceGoal.Deserialize(deserializer);
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
