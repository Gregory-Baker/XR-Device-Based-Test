using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Std;
using RosMessageTypes.Actionlib;

namespace RosMessageTypes.Mbf
{
    public class MoveBaseActionGoal : ActionGoal<MoveBaseGoal>
    {
        public const string k_RosMessageName = "mbf_msgs/MoveBaseActionGoal";
        public override string RosMessageName => k_RosMessageName;


        public MoveBaseActionGoal() : base()
        {
            this.goal = new MoveBaseGoal();
        }

        public MoveBaseActionGoal(HeaderMsg header, GoalIDMsg goal_id, MoveBaseGoal goal) : base(header, goal_id)
        {
            this.goal = goal;
        }
        public static MoveBaseActionGoal Deserialize(MessageDeserializer deserializer) => new MoveBaseActionGoal(deserializer);

        MoveBaseActionGoal(MessageDeserializer deserializer) : base(deserializer)
        {
            this.goal = MoveBaseGoal.Deserialize(deserializer);
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
