using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.KinovaCustom
{
    public class MoveDistanceAction : Action<MoveDistanceActionGoal, MoveDistanceActionResult, MoveDistanceActionFeedback, MoveDistanceGoal, MoveDistanceResult, MoveDistanceFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/MoveDistanceAction";
        public override string RosMessageName => k_RosMessageName;


        public MoveDistanceAction() : base()
        {
            this.action_goal = new MoveDistanceActionGoal();
            this.action_result = new MoveDistanceActionResult();
            this.action_feedback = new MoveDistanceActionFeedback();
        }

        public static MoveDistanceAction Deserialize(MessageDeserializer deserializer) => new MoveDistanceAction(deserializer);

        MoveDistanceAction(MessageDeserializer deserializer)
        {
            this.action_goal = MoveDistanceActionGoal.Deserialize(deserializer);
            this.action_result = MoveDistanceActionResult.Deserialize(deserializer);
            this.action_feedback = MoveDistanceActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
