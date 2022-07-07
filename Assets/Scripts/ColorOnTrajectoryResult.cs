using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Moveit;
using System;
using System.Linq;

public class ColorOnTrajectoryResult : MonoBehaviour
{
    ROSConnection ros;

    [SerializeField]
    string topicName = null;

    public Color32 goalSuccessColour = Color.green;
    public Color32 goalFailureColour = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<ExecuteTrajectoryActionResult>(topicName, ColorChangeCallback);
    }

    private void ColorChangeCallback(ExecuteTrajectoryActionResult msg)
    {
        var status = msg.status.status;

        if (status == 3)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = goalSuccessColour;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = goalFailureColour;
        }
    }
}
