using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    public ParticipantHandler participantHandler;

    public GameObject[] directOnlyObjects;
    public GameObject[] waypointOnlyObjects;

    // Start is called before the first frame update
    void Start()
    {
        if(participantHandler.controlMethod == ParticipantHandler.ControlMethod.Direct)
        {
            foreach (GameObject obj in waypointOnlyObjects)
            {
                obj.SetActive(false);
            }
        }
        if (participantHandler.controlMethod == ParticipantHandler.ControlMethod.Waypoint)
        {
            foreach (GameObject obj in directOnlyObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
