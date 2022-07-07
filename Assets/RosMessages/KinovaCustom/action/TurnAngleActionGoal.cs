using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class TurnAngleActionGoal : ActionGoal<TurnAngleGoal>
    {
        public const string k_RosMessageName = "kinova_custom_actions/TurnAngleActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public TurnAngleActionGoal() : base()
        {
            this.goal = new TurnAngleGoal();
        }

        public TurnAngleActionGoal(HeaderMsg header, GoalIDMsg goal_id, TurnAngleGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static TurnAngleActionGoal Deserialize(MessageDeserializer deserializer) => new TurnAngleActionGoal(deserializer);

        TurnAngleActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = TurnAngleGoal.Deserialize(deserializer);
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
