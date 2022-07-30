using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotationPubHandler : MonoBehaviour
{
    RosHeadRotationPublisher headRotationPublisher;

    ParticipantHandler participantHandler;

    public float tiltOffsetScreen = 0f;
    public float tiltOffsetVR = 0f;

    // Start is called before the first frame update
    void Start()
    {
        participantHandler = FindObjectOfType<ParticipantHandler>();

        headRotationPublisher = GetComponent<RosHeadRotationPublisher>();


        switch (participantHandler.teleopInterface)
        {
            case ParticipantHandler.TeleopInterface.VR:
                headRotationPublisher.trackHead = true;
                headRotationPublisher.tiltOffset = tiltOffsetVR;
                break;
            case ParticipantHandler.TeleopInterface.Screen:
                headRotationPublisher.trackHead = false;
                headRotationPublisher.tiltOffset = tiltOffsetScreen;
                break;
        }

        if (participantHandler.tutorial)
        {
            headRotationPublisher.trackHead = false;
            headRotationPublisher.tiltOffset = tiltOffsetScreen;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
