using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ArmControllerSteer : MonoBehaviour
{
    [Header("Events")]
    public ChangeArmTargetControl changeArmTargetControl;

    [Header("External Objects")]
    public GameObject targetObject;
    public GameObject arrows;

    [Header("SteamVR Inputs")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Boolean changeControlMode = null;
    public SteamVR_Action_Boolean confirmTarget = null;
    public SteamVR_Action_Boolean actuateGripper = null;
    public SteamVR_Action_Vector2 moveTargetInPlane = null;
    public SteamVR_Action_Vector2 moveTargetUpDownRotate = null;
    public SteamVR_Action_Boolean noResponse = null;
    public SteamVR_Action_Boolean yesResponse = null;
    public SteamVR_Action_Boolean stopArm = null;

    [Header("Parameters")]
    public bool targetOrientationChangeable = false;
    public float axisThreshold = 0.5f;
    public float moveSpeed = 0.1f;
    public float directionTolerance = 0.01f;
    public float turnSpeed = 50f;

    [Header("Target Limits")]
    public float targetForwardMax = 0.8f;
    public float targetForwardMin = 0.0f;
    public float targetLRMax = 0.4f;
    public float targetHeightMax = 0.8f;


    public UnityEvent onStopEvents;
    public UnityEvent onConfirmTargetEvents;
    public UnityEvent onActuateGripperEvents;
    public UnityEvent yesResponseEvents;
    public UnityEvent noResponseEvents;



    private void OnEnable()
    {
        changeControlMode[inputSource].onStateDown += ChangeControlMode;
        confirmTarget[inputSource].onStateDown += ConfirmTarget;
        actuateGripper[inputSource].onStateDown += ActuateGripper;
        moveTargetInPlane[inputSource].onAxis += MoveTargetInPlane;
        moveTargetUpDownRotate[inputSource].onAxis += MoveTargetUpDownRotate;
        noResponse[inputSource].onStateDown += TriggerNoResponseEvents;
        yesResponse[inputSource].onStateDown += TriggerYesResponseEvents;
        stopArm[inputSource].onStateDown += StopArm;

        SetControlModeToPosition();
    }

    private void OnDisable()
    {
        changeControlMode[inputSource].onStateDown -= ChangeControlMode;
        confirmTarget[inputSource].onStateDown -= ConfirmTarget;
        actuateGripper[inputSource].onStateDown -= ActuateGripper;
        moveTargetInPlane[inputSource].onAxis -= MoveTargetInPlane;
        moveTargetUpDownRotate[inputSource].onAxis -= MoveTargetUpDownRotate;
        noResponse[inputSource].onStateDown -= TriggerNoResponseEvents;
        yesResponse[inputSource].onStateDown -= TriggerYesResponseEvents;
        stopArm[inputSource].onStateDown -= StopArm;
    }

    private void TriggerYesResponseEvents(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.ConfirmActionSuccess)
            yesResponseEvents.Invoke();
    }

    private void TriggerNoResponseEvents(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.ConfirmActionSuccess)
            noResponseEvents.Invoke();
    }

    public void SetControlModeToPosition() {
        changeArmTargetControl.SetControlMode(ChangeArmTargetControl.ControlMode.TargetPositionControl);
    }

    private void StopArm(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onStopEvents.Invoke();
    }

    private void ActuateGripper(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onActuateGripperEvents.Invoke();
    }

    private void ConfirmTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onConfirmTargetEvents.Invoke();
    }

    private void ChangeControlMode(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        changeArmTargetControl.ChangeControlMode();
        // changeArmTargetControl.TriggerEvent();
    }

    private void MoveTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (axis.magnitude > axisThreshold)
        {
            switch (changeArmTargetControl.controlMode)
            {
                case ChangeArmTargetControl.ControlMode.SpecialFunctions:
                    turnTarget(axis);
                    moveTargetUpDown(axis);
                    break;
                case ChangeArmTargetControl.ControlMode.TargetPositionControl:
                    moveTargetFBLR(axis);
                    break;
            }
        }
    }

    private void MoveTargetInPlane(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        // if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.TargetPositionControl)
            moveTargetFBLR(axis);
    }

    private void moveTargetFBLR(Vector2 axis)
    {
        Vector3 translation = new Vector3();
        if (axis.y > 0)
        {
            if (targetObject.transform.localPosition.z < targetForwardMax)
                translation.z = axis.y * moveSpeed * Time.deltaTime;
        }
        else if (axis.y < 0)
        {
            if (targetObject.transform.localPosition.z > targetForwardMin)
                translation.z = axis.y * moveSpeed * Time.deltaTime;
        }

        if (axis.x > 0)
        {
            if (targetObject.transform.localPosition.x < targetLRMax)
                translation.x = axis.x * moveSpeed * Time.deltaTime;
        }
        if (axis.x < 0)
            if (targetObject.transform.localPosition.x > -targetLRMax)
                translation.x = axis.x * moveSpeed * Time.deltaTime;

        targetObject.transform.Translate(translation, targetObject.transform.parent);
    }

    private void MoveTargetUpDownRotate(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.TargetPositionControl)
        {
            if (targetOrientationChangeable)
            {
                turnTarget(axis);
            }
            
            moveTargetUpDown(axis);
        }
    }

    private void moveTargetUpDown(Vector2 axis)
    {
        if (Mathf.Abs(axis.y) > axisThreshold) {
            Vector3 translation = new Vector3(0, axis.y * moveSpeed * Time.deltaTime, 0);
            targetObject.transform.Translate(translation, Space.World);
        }
    }


    private void turnTarget(Vector2 axis)
    {
        if (Mathf.Abs(axis.x) > axisThreshold)
        {
            Vector3 rotation = new Vector3(0, -axis.x * turnSpeed * Time.deltaTime, 0);
            targetObject.transform.Rotate(rotation);
            arrows.transform.Rotate(-rotation);
        }
    }
}