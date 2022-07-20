using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayHandler : MonoBehaviour
{
    public ParticipantHandler participantHandler;

    public GameObject[] screenOverlayObjects;
    public GameObject[] vrOverlayObjects;

    // Start is called before the first frame update
    void Start()
    {
        if (participantHandler.teleopInterface == ParticipantHandler.TeleopInterface.VR)
        {
            foreach (var overlay in screenOverlayObjects)
            {
                overlay.SetActive(false);
            }
        }

        if (participantHandler.teleopInterface == ParticipantHandler.TeleopInterface.Screen)
        {
            foreach (var overlay in vrOverlayObjects)
            {
                overlay.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
