using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchInput : MonoBehaviour
{
    public SteamVR_ActionSet baseActionSet;
    public SteamVR_ActionSet armActionSet;

    public void LoadBaseInput()
    {
        baseActionSet.Activate();
        armActionSet.Deactivate();
    }

    public void LoadArmInput()
    {
        armActionSet.Activate();
        baseActionSet.Deactivate();
    }
}
