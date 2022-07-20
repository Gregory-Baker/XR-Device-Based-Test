using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    KeyboardTeleop inputActions;

    [HideInInspector]
    public static CurrentControlSet controlSet = CurrentControlSet.Base;

    private void Awake()
    {
        inputActions = new KeyboardTeleop();
    }


    private void OnEnable()
    {
        inputActions.Common.Enable();
        inputActions.Common.ChangeActionSet.started += changeActionSet;
        changeActionSetAction[SteamVR_Input_Sources.Any].onStateDown += changeActionSet;
    }

    private void OnDisable()
    {
        inputActions.Common.Disable();
        inputActions.Common.ChangeActionSet.started -= changeActionSet;
        changeActionSetAction[SteamVR_Input_Sources.Any].onStateDown -= changeActionSet;
    }

    private void Start()
    {
        LoadBaseController();
    }

    public void LoadArmController()
    {
        loadArmControllerEvent.TriggerEvent();
        controlSet = CurrentControlSet.Arm;
    }

    public void LoadBaseController()
    {
        loadBaseControllerEvent.TriggerEvent();
        controlSet = CurrentControlSet.Base;
    }

    private void changeActionSet(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ChangeActionSet();
    }

    private void changeActionSet(InputAction.CallbackContext obj)
    {
        ChangeActionSet();
    }

    private void ChangeActionSet()
    {
        switch (controlSet)
        {
            case CurrentControlSet.Base:
                LoadArmController();
                break;
            case CurrentControlSet.Arm:
                LoadBaseController();
                break;
            default:
                throw new Exception("Undefined controller state");
        }
    }


}
