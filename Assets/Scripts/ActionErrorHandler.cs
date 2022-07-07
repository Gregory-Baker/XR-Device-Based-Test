using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using System.Linq;
using UnityEngine.Events;

public class ActionErrorHandler : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string topicName = null;

    [SerializeField]
    UnityEvent onErrorEvents;

    public enum ActionStatus
    {
        FAILED,
        IN_PROGRESS,
        SUCCEEDED
    }

    public ActionStatus actionStatus = ActionStatus.SUCCEEDED;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<GoalStatusArrayMsg>(topicName, ColorChangeCallback);
    }

    private void ColorChangeCallback(GoalStatusArrayMsg msg)
    {
        if (msg.status_list.Length > 0)
        {
            var status = msg.status_list.Last().status;

                onErrorEvents.Invoke();

        }
    }
}
