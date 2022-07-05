using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;


namespace RosMessageTypes.KinovaCustom
{
    public class PlaceObjectAction : Action<PlaceObjectActionGoal, PlaceObjectActionResult, PlaceObjectActionFeedback, PlaceObjectGoal, PlaceObjectResult, PlaceObjectFeedback>
    {
        public const string k_RosMessageName = "kinova_custom_actions/PlaceObjectAction";
        public override string RosMessageName => k_RosMessageName;


        public PlaceObjectAction() : base()
        {
            this.action_goal = new PlaceObjectActionGoal();
            this.action_result = new PlaceObjectActionResult();
            this.action_feedback = new PlaceObjectActionFeedback();
        }

        public static PlaceObjectAction Deserialize(MessageDeserializer deserializer) => new PlaceObjectAction(deserializer);

        PlaceObjectAction(MessageDeserializer deserializer)
        {
            this.action_goal = PlaceObjectActionGoal.Deserialize(deserializer);
            this.action_result = PlaceObjectActionResult.Deserialize(deserializer);
            this.action_feedback = PlaceObjectActionFeedback.Deserialize(deserializer);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.action_goal);
            serializer.Write(this.action_result);
            serializer.Write(this.action_feedback);
        }

    }
}
