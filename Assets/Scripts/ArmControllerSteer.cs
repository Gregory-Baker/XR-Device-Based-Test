using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ArmControllerSteer : MonoBehaviour
{
    public enum ControlMode
    {
        UpDown,
        ForwardBackLeftRight,
        RotateZ
    }

    [HideInInspector]
    public ControlMode controlMode = ControlMode.UpDown;

    public GameObject targetObject;
    public GameObject upArrows;
    public GameObject fblrArrows;
    public GameObject rotationArrows;

    [Header("SteamVR Inputs")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Boolean changeControlMode = null;
    public SteamVR_Action_Boolean confirmTarget = null;
    public SteamVR_Action_Boolean actuateGripper = null;
    public SteamVR_Action_Vector2 moveTarget = null;

    float axisThreshold = 0.2f;
    public float moveDistance = 0.01f;
    public float directionTolerance = 0.01f;
    public float turnAngle = 0.05f;
    float directionLast = 0;

    public UnityEvent onConfirmTargetEvents;
    public UnityEvent onActuateGripperEvents;


    private void OnEnable()
    {
        changeControlMode[inputSource].onStateDown += ChangeControlMode;
        confirmTarget[inputSource].onStateDown += ConfirmTarget;
        actuateGripper[inputSource].onStateDown += ActuateGripper;
        moveTarget[inputSource].onAxis += MoveTarget;
        ShowArrows();
    }

    private void OnDisble()
    {
        changeControlMode[inputSource].onStateDown -= ChangeControlMode;
        confirmTarget[inputSource].onStateDown -= ConfirmTarget;
        actuateGripper[inputSource].onStateDown -= ActuateGripper;
        moveTarget[inputSource].onAxis -= MoveTarget;
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
        controlMode = (controlMode != ControlMode.RotateZ) ? controlMode + 1 : 0;

        ShowArrows();

    }

    private void MoveTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (axis.magnitude > axisThreshold)
        {
            switch (controlMode)
            {
                case ControlMode.UpDown:
                    moveTargetUpDown(axis);
                    break;
                case ControlMode.ForwardBackLeftRight:
                    moveTargetFBLR(axis);
                    break;
                case ControlMode.RotateZ:
                    turnTarget(axis);
                    break;
            }
        }
    }

    private void moveTargetFBLR(Vector2 axis)
    {
        Vector3 translation = new Vector3(axis.x * moveDistance, 0, axis.y * moveDistance);
        targetObject.transform.Translate(translation, targetObject.transform.parent);
    }

    private void moveTargetUpDown(Vector2 axis)
    {
        Vector3 translation = new Vector3(0, axis.y * moveDistance, 0);
        targetObject.transform.Translate(translation, Space.World);
    }

    private void turnTarget(Vector2 axis)
    {
        float direction = Mathf.Atan2(axis.y, axis.x);
        float directionDelta = direction - directionLast;
        if (Mathf.Abs(directionDelta) < Mathf.PI && Mathf.Abs(directionDelta) > directionTolerance)
        {
            targetObject.transform.Rotate(new Vector3(0, Mathf.Sign(directionDelta) * turnAngle / Time.deltaTime, 0));
            fblrArrows.transform.Rotate(new Vector3(0, -Mathf.Sign(directionDelta) * turnAngle / Time.deltaTime, 0));
        }
        directionLast = direction;
    }

    public void ShowArrows()
    {
        switch (controlMode)
        {
            case ControlMode.UpDown:
                upArrows.SetActive(true);
                fblrArrows.SetActive(false);
                rotationArrows.SetActive(false);
                break;
            case ControlMode.ForwardBackLeftRight:
                upArrows.SetActive(false);
                fblrArrows.SetActive(true);
                rotationArrows.SetActive(false);
                break;
            case ControlMode.RotateZ:
                upArrows.SetActive(false);
                fblrArrows.SetActive(false);
                rotationArrows.SetActive(true);
                break;
        }
    }
}