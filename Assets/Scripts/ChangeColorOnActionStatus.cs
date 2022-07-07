using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using System;
using System.Linq;

public class ChangeColorOnActionStatus : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string topicName = null;

    public Color32 goalSuccessColour = Color.green;
    public Color32 goalFailureColour = Color.red;
    public Color32 goalActiveColour = Color.yellow;

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
        if (msg.status_list.Length > 0) {
            var status = msg.status_list.Last().status;
            if (status == 3)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = goalSuccessColour;
                actionStatus = ActionStatus.SUCCEEDED;
            }
            else if (status == 1)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = goalActiveColour;
                actionStatus = ActionStatus.IN_PROGRESS;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.color = goalFailureColour;
                actionStatus = ActionStatus.FAILED;
            }
        }

    }
}
