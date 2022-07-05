using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.KinovaCustom
{
    public class PlaceObjectActionGoal : ActionGoal<PlaceObjectGoal>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PlaceObjectActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public PlaceObjectActionGoal() : base()
        {
            this.goal = new PlaceObjectGoal();
        }

        public PlaceObjectActionGoal(HeaderMsg header, GoalIDMsg goal_id, PlaceObjectGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static PlaceObjectActionGoal Deserialize(MessageDeserializer deserializer) => new PlaceObjectActionGoal(deserializer);

        PlaceObjectActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = PlaceObjectGoal.Deserialize(deserializer);
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
