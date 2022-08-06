using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.KinovaCustom
{
    public class PickObjectFullAction : Action<PickObjectFullActionGoal, PickObjectFullActionResult, PickObjectFullActionFeedback, PickObjectFullGoal, PickObjectFullResult, PickObjectFullFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PickObjectFullAction";
        public override string RosMessageName => k_RosMessageName;


        public PickObjectFullAction() : base()
        {
            this.action_goal = new PickObjectFullActionGoal();
            this.action_result = new PickObjectFullActionResult();
            this.action_feedback = new PickObjectFullActionFeedback();
        }

        public static PickObjectFullAction Deserialize(MessageDeserializer deserializer) => new PickObjectFullAction(deserializer);

        PickObjectFullAction(MessageDeserializer deserializer)
        {
            this.action_goal = PickObjectFullActionGoal.Deserialize(deserializer);
            this.action_result = PickObjectFullActionResult.Deserialize(deserializer);
            this.action_feedback = PickObjectFullActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
