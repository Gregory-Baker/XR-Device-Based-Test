using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using Valve.VR;

public class ActionSetHandler : MonoBehaviour
{
    [HideInInspector]
    public enum CurrentControlSet
    {
        Base,
        Arm
    }


    [Header("Change Action Set")]
    public LoadBaseController loadBaseControllerEvent;
    public LoadArmController loadArmControllerEvent;
    public SteamVR_Action_Boolean changeActionSetAction;

    [HideInInspector]
    public static CurrentControlSet controlSet = CurrentControlSet.Base;

    private void OnEnable()
    {
        changeActionSetAction[SteamVR_Input_Sources.Any].onStateDown += changeActionSet;
    }

    private void OnDisable()
    {
        changeActionSetAction[SteamVR_Input_Sources.Any].onStateDown += changeActionSet;
    }

    private void changeActionSet(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (controlSet)
        {
            case CurrentControlSet.Base:
                loadArmControllerEvent.TriggerEvent();
                controlSet = CurrentControlSet.Arm;
                break;
            case CurrentControlSet.Arm:
                loadBaseControllerEvent.TriggerEvent();
                controlSet = CurrentControlSet.Base;
                break;
            default:
                throw new Exception("Undefined controller state");
        }        
    }
}
