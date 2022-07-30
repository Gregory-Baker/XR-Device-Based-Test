using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceHandler : MonoBehaviour
{
    public ParticipantHandler participantHandler;

    public GameObject[] screenOnlyObjects;
    public GameObject[] vrOnlyObjects;

    // Start is called before the first frame update
    void Start()
    {
        if (participantHandler.teleopInterface == ParticipantHandler.TeleopInterface.VR)
        {
            foreach (var overlay in screenOnlyObjects)
            {
                overlay.SetActive(false);
            }
        }

        if (participantHandler.teleopInterface == ParticipantHandler.TeleopInterface.Screen)
        {
            foreach (var overlay in vrOnlyObjects)
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
