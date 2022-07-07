using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using System;
using System.Linq;

public class ColorOnActive : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string[] topicNames = null;

    public Color32 goalActiveColour = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        foreach (var topicName in topicNames)
            ros.Subscribe<GoalStatusArrayMsg>(topicName, ColorChangeCallback);
    }

    private void ColorChangeCallback(GoalStatusArrayMsg msg)
    {
        if (msg.status_list.Length > 0)
        {
            var status = msg.status_list.Last().status;

            if (status == 1)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = goalActiveColour;
            }
        }
    }
}
