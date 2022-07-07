using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : TrackObject
{
    [SerializeField]
    RosHeadRotationPublisher headRotationPublisher;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationOffset = headRotationPublisher.panOffset;
    }
}
