using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaypointController : MonoBehaviour
{

    public XRBaseController xrController;

    public InputHelpers.Button teleportActivationButton;

    public float activationThreshold = 0.1f;

    // Update is called once per frame
    void Update()
    {

    }
}
