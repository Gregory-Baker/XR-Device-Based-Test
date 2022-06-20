using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using System;

public class ColorOnError : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string topicName = "/execute_trajectory/status";

    public Color32 goalSuccessColour = Color.green;
    public Color32 goalFailureColour = Color.red;
    public Color32 goalActiveColour = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<GoalStatusArrayMsg>(topicName, ColorChangeCallback);
    }

    private void ColorChangeCallback(GoalStatusArrayMsg msg)
    {
        var status = msg.status_list[0].status;
        if (status == 3)
        {
            Debug.Log("Arm Move Success");
            gameObject.GetComponent<MeshRenderer>().material.color = goalSuccessColour;
        }
        if (status == 1)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = goalActiveColour;
        }
        else
        {
            Debug.Log("Arm Move Failure");
            gameObject.GetComponent<MeshRenderer>().material.color = goalFailureColour;
        }
    }
}
