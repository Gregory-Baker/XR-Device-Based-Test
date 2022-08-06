using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using Valve.VR;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetRayVelocityHandler : MonoBehaviour
{
    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Pose poseAction = null;

    public XRRayInteractor xrRayInteractor;

    public float minRayVelocity = 5f;
    public float maxRayVelocity = 15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = poseAction[inputSource].localRotation;
        Vector3 angles = rotation.eulerAngles;

        float velocityScaler;

        if (angles.x < 180)
        {
            velocityScaler = 1.0f;
        }
        else if(angles.x < 270)
        {
            velocityScaler = (270 - angles.x) / 90;
        }
        else
        {
            velocityScaler = (angles.x - 270) / 90;
        }

        float lerpVelocity = Mathf.Lerp(minRayVelocity, maxRayVelocity, velocityScaler);

        xrRayInteractor.velocity = lerpVelocity;
    }
}
