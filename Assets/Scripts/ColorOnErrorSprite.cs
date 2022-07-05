using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Actionlib;
using System.Linq;

public class ColorOnErrorSprite : MonoBehaviour
{

    ROSConnection ros;

    [SerializeField]
    string topicName = null;

    public Color32 goalSuccessColour = Color.green;
    public Color32 goalFailureColour = Color.red;
    public Color32 goalActiveColour = Color.yellow;

    SpriteRenderer spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<GoalStatusArrayMsg>(topicName, ColorChangeCallback);
    }

    private void ColorChangeCallback(GoalStatusArrayMsg msg)
    {
        if (msg.status_list.Length > 0)
        {
            var status = msg.status_list.Last().status;
            if (status == 3)
            {
                spriteRenderer.color = goalSuccessColour;
            }
            else if (status == 1)
            {
                spriteRenderer.color = goalActiveColour;
            }
            else
            {
                spriteRenderer.color = goalFailureColour;
            }
        }
    }



}
