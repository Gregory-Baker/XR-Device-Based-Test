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
        UpDownRotate,
        ForwardBackLeftRight,
        Place
    }

    [HideInInspector]
    public ControlMode controlMode = ControlMode.UpDownRotate;

    public GameObject targetObject;
    public GameObject upRotateArrows;
    public GameObject fblrArrows;
    public GameObject placeVerticalAxis;

    [Header("SteamVR Inputs")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Boolean changeControlMode = null;
    public SteamVR_Action_Boolean confirmTarget = null;
    public SteamVR_Action_Boolean actuateGripper = null;
    public SteamVR_Action_Vector2 moveTarget = null;

    public float axisThreshold = 0.5f;
    public float moveDistance = 0.01f;
    public float directionTolerance = 0.01f;
    public float turnAngle = 0.05f;
    float directionLast = 0;
    float yAxisLast = 0;

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
        controlMode = (controlMode != ControlMode.Place) ? controlMode + 1 : 0;

        ShowArrows();

    }

    private void MoveTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (axis.magnitude > axisThreshold)
        {
            switch (controlMode)
            {
                case ControlMode.UpDownRotate:
                    turnTarget(axis);
                    moveTargetUpDown(axis);
                    break;
                case ControlMode.ForwardBackLeftRight:
                    moveTargetFBLR(axis);
                    break;
                case ControlMode.Place:
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
        if (Mathf.Abs(axis.y) > axisThreshold) {
            Vector3 translation = new Vector3(0, axis.y * moveDistance, 0);
            targetObject.transform.Translate(translation, Space.World);
        }
    }

    private void moveTargetUpDown(Vector2 axis, Vector2 delta)
    {
        Vector3 translation = new Vector3(0, delta.y * moveDistance, 0);
        targetObject.transform.Translate(translation, Space.World);
    }

    //private bool turnTarget(Vector2 axis)
    //{
    //    bool turned = false;
    //    float direction = Mathf.Atan2(axis.y, axis.x);
    //    float directionDelta = direction - directionLast;
    //    if (Mathf.Abs(directionDelta) < Mathf.PI && Mathf.Abs(directionDelta) > directionTolerance)
    //    {
    //        targetObject.transform.Rotate(new Vector3(0, Mathf.Sign(directionDelta) * turnAngle / Time.deltaTime, 0));
    //        fblrArrows.transform.Rotate(new Vector3(0, -Mathf.Sign(directionDelta) * turnAngle / Time.deltaTime, 0));
    //        turned = true;
    //    }
    //    directionLast = direction;
    //    return turned;
    //}

    private void turnTarget(Vector2 axis)
    {
        if (Mathf.Abs(axis.x) > axisThreshold)
        {
            targetObject.transform.Rotate(new Vector3(0, -axis.x * turnAngle / Time.deltaTime, 0));
        }
    }

    public void ShowArrows()
    {
        switch (controlMode)
        {
            case ControlMode.UpDownRotate:
                upRotateArrows.SetActive(true);
                fblrArrows.SetActive(false);
                placeVerticalAxis.SetActive(false);
                break;
            case ControlMode.ForwardBackLeftRight:
                upRotateArrows.SetActive(false);
                fblrArrows.SetActive(true);
                placeVerticalAxis.SetActive(false);
                break;
            case ControlMode.Place:
                upRotateArrows.SetActive(false);
                fblrArrows.SetActive(false);
                placeVerticalAxis.SetActive(true);
                break;
        }
    }
}