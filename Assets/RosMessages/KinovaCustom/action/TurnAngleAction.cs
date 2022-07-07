using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.KinovaCustom
{
    public class TurnAngleAction : Action<TurnAngleActionGoal, TurnAngleActionResult, TurnAngleActionFeedback, TurnAngleGoal, TurnAngleResult, TurnAngleFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/TurnAngleAction";
        public override string RosMessageName => k_RosMessageName;


        public TurnAngleAction() : base()
        {
            this.action_goal = new TurnAngleActionGoal();
            this.action_result = new TurnAngleActionResult();
            this.action_feedback = new TurnAngleActionFeedback();
        }

        public static TurnAngleAction Deserialize(MessageDeserializer deserializer) => new TurnAngleAction(deserializer);

        TurnAngleAction(MessageDeserializer deserializer)
        {
            this.action_goal = TurnAngleActionGoal.Deserialize(deserializer);
            this.action_result = TurnAngleActionResult.Deserialize(deserializer);
            this.action_feedback = TurnAngleActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
