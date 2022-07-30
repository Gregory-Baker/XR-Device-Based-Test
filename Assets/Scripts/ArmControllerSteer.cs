using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Valve.VR;

public class ArmControllerSteer : MonoBehaviour
{
    KeyboardTeleop inputActions;

    [Header("Events")]
    public ChangeArmTargetControl changeArmTargetControl;

    [Header("External Objects")]
    public GameObject targetObject;
    public GameObject arrows;
    public TMP_Dropdown dropdown;

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
    public float targetHeightMin = 0.0f;


    public UnityEvent onStopEvents;
    public UnityEvent onConfirmTargetEvents;
    public UnityEvent onActuateGripperEvents;
    public UnityEvent yesResponseEvents;
    public UnityEvent noResponseEvents;
    public UnityEvent pickEvents;
    public UnityEvent placeEvents;
    public UnityEvent armToHomeEvents;
    public UnityEvent actuateGripperEvents;
    public UnityEvent openGripperEvents;
    public UnityEvent closeGripperEvents;


    private void Awake()
    {
        inputActions = new KeyboardTeleop();

        inputActions.Arm.FBLR.started += MoveTargetInPlaneKeyboard;
        inputActions.Arm.UpDown.started += MoveTargetUpDownRotate;
        inputActions.Arm.ConfirmTarget.started += ConfirmTarget;
        inputActions.Arm.StopArm.started += StopArm;
        inputActions.Arm.ArmToHome.started += ArmToHome;

        changeControlMode[inputSource].onStateDown += ChangeControlMode;
        confirmTarget[inputSource].onStateDown += ConfirmTarget;
        actuateGripper[inputSource].onStateDown += ActuateGripper;
        moveTargetInPlane[inputSource].onAxis += MoveTargetInPlane;
        moveTargetUpDownRotate[inputSource].onAxis += MoveTargetUpDownRotate;
        noResponse[inputSource].onStateDown += TriggerNoResponseEvents;
        yesResponse[inputSource].onStateDown += TriggerYesResponseEvents;
        stopArm[inputSource].onStateDown += StopArm;
    }

    private void OnEnable()
    {
        inputActions.Arm.Enable();

        SetControlModeToPosition();
    }

    private void OnDisable()
    {
        inputActions.Arm.Disable();
    }

    private void TriggerYesResponseEvents(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        SuccessfulActionResponse();
    }

    public void SuccessfulActionResponse()
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.ConfirmActionSuccess)
            yesResponseEvents.Invoke();
        dropdown.value = 0;
    }

    private void TriggerNoResponseEvents(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        FailedActionResponse();
    }

    public void FailedActionResponse()
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.ConfirmActionSuccess)
            noResponseEvents.Invoke();
        dropdown.value = 0;
    }

    public void SetControlModeToPosition()
    {
        changeArmTargetControl.SetControlMode(ChangeArmTargetControl.ControlMode.TargetPositionControl);
    }

    private void StopArm(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onStopEvents.Invoke();
    }

    private void StopArm(InputAction.CallbackContext obj)
    {
        onStopEvents.Invoke();
    }

    private void ArmToHome(InputAction.CallbackContext obj)
    {
        ArmToHome();
    }

    private void ArmToHome()
    {
        armToHomeEvents.Invoke();
    }

    private void ActuateGripper(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onActuateGripperEvents.Invoke();
    }

    public IEnumerator ResetDropdownCoroutine (float delay)
    {
        yield return new WaitForSeconds(delay);
        dropdown.value = 0;
    }

    private void ConfirmTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        onConfirmTargetEvents.Invoke();
    }

    private void ConfirmTarget(InputAction.CallbackContext obj)
    {
        onConfirmTargetEvents.Invoke();
    }

    public void PerformAction(int action)
    {
        switch (action)
        {
            case 0:
                break;
            case 1:
                pickEvents.Invoke();
                break;
            case 2:
                placeEvents.Invoke();
                break;
            case 3:
                ArmToHome();
                StartCoroutine(ResetDropdownCoroutine(4.0f));
                break;
            case 4:
                openGripperEvents.Invoke();
                StartCoroutine(ResetDropdownCoroutine(1.5f));
                break;
            case 5:
                closeGripperEvents.Invoke();
                StartCoroutine(ResetDropdownCoroutine(1.5f));
                break;
        }
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
                    moveTargetUpDown(axis.y);
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

    private void MoveTargetInPlaneKeyboard(InputAction.CallbackContext obj)
    {

        StartCoroutine(MoveTargetInPlaneKeyboardCoroutine(obj));
    }

    private IEnumerator MoveTargetInPlaneKeyboardCoroutine(InputAction.CallbackContext obj)
    {
        var axis = obj.ReadValue<Vector2>();
        while (axis.magnitude > 0.1)
        {
            axis = obj.ReadValue<Vector2>();
            moveTargetFBLR(axis);
            yield return null;
        }
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

            moveTargetUpDown(axis.y);
        }
    }

    private void MoveTargetUpDownRotate(InputAction.CallbackContext obj)
    {
        if (changeArmTargetControl.controlMode == ChangeArmTargetControl.ControlMode.TargetPositionControl)
        {

            StartCoroutine(MoveTargetUpDownRotateCoroutine(obj));
        }
    }

    private IEnumerator MoveTargetUpDownRotateCoroutine(InputAction.CallbackContext obj)
    {
        var axis = obj.ReadValue<float>();
        while (Mathf.Abs(axis) > 0.1)
        {
            axis = obj.ReadValue<float>();
            moveTargetUpDown(axis);
            yield return null;
        }
    }

    private void moveTargetUpDown(float axis)
    {
        if (Mathf.Abs(axis) > axisThreshold)
        {
            Vector3 translation = Vector3.zero;
            if (axis > 0)
            {
                if (targetObject.transform.localPosition.y < targetHeightMax)
                    translation.y = axis * moveSpeed * Time.deltaTime;
            }
            else if (axis < 0)
            {
                if (targetObject.transform.localPosition.y > targetHeightMin)
                    translation.y = axis * moveSpeed * Time.deltaTime;
            }
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