using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class BaseController : MonoBehaviour
{
    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    public SteamVR_Action_Boolean targetSelectAction = null;

    public GameObject xrRayObject = null;
    private XRRayInteractor xrRayInteractor = null;
    private XRInteractorLineVisual xrLineVisual = null;

    [Header("SteamVR Input")]
    public SteamVR_Action_Vector2 targetRotation = null;
    public float directionTolerance = 0.01f;
    public float turnMultiplier = 20;
    float directionLast = 0;

    public SteamVR_Action_Vector2 strafeTargetAction = null;

    public SteamVR_Action_Boolean targetConfirmAction = null;
    public delegate void ConfirmAction();
    public static event ConfirmAction OnConfirm;

    public SteamVR_Action_Boolean turnLeftAction = null;
    public delegate void TurnLeftAction();
    public static event TurnLeftAction OnTurnLeft;

    //public SteamVR_Action_Boolean nudgeForwardAction = null;
    public delegate void NudgeForwardAction();
    public static event NudgeForwardAction OnNudgeForward;

    //public SteamVR_Action_Boolean nudgeBackwardAction = null;
    public delegate void NudgeBackwardAction();
    public static event NudgeBackwardAction OnNudgeBackward;

    public SteamVR_Action_Boolean turnRightAction = null;
    public delegate void TurnRightAction();
    public static event TurnLeftAction OnTurnRight;

    public SteamVR_Action_Boolean stopAction = null;
    public delegate void StopAction();
    public static event TurnLeftAction OnStop;

    [Header("External Objects")]
    public RosHeadRotationPublisher panTiltPublisher;

    // Start is called before the first frame update
    void OnEnable()
    {
        xrRayInteractor = xrRayObject.GetComponent<XRRayInteractor>();
        xrLineVisual = xrRayObject.GetComponent<XRInteractorLineVisual>();

        targetSelectAction[inputSource].onStateDown += enableTargetSelection;
        targetSelectAction[inputSource].onStateUp += disableTargetSelection;

        targetRotation[inputSource].onAxis += turnTarget;
        strafeTargetAction[inputSource].onAxis += strafeTarget;
        targetConfirmAction[inputSource].onStateDown += confirmTarget;
        turnLeftAction[inputSource].onStateDown += turnLeft;
        turnRightAction[inputSource].onStateDown += turnRight;
        //nudgeForwardAction[inputSource].onStateDown += nudgeForward;
        //nudgeBackwardAction[inputSource].onStateDown += nudgeBackward;
        stopAction[inputSource].onStateDown += stopRobot;
    }

    // Update is called once per frame
    void OnDisable()
    {
        targetSelectAction[inputSource].onStateDown -= enableTargetSelection;
        targetSelectAction[inputSource].onStateDown -= disableTargetSelection;

        targetRotation[inputSource].onAxis -= turnTarget;
        strafeTargetAction[inputSource].onAxis -= strafeTarget;
        targetConfirmAction[inputSource].onStateDown -= confirmTarget;
        turnLeftAction[inputSource].onStateDown -= turnLeft;
        turnRightAction[inputSource].onStateDown -= turnRight;
        //nudgeForwardAction[inputSource].onStateDown -= nudgeForward;
        //nudgeBackwardAction[inputSource].onStateDown -= nudgeBackward;
        stopAction[inputSource].onStateDown -= stopRobot;
    }



    private void disableTargetSelection(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (xrRayObject != null)
        {
            xrRayObject.SetActive(false);
        }
    }

    private void enableTargetSelection(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (xrRayObject != null)
        {
            xrRayObject.SetActive(true);
        }
    }

    private void turnTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (axis.magnitude > 0)
        {
            float direction = -Mathf.Atan2(targetRotation[inputSource].axis.y, targetRotation[inputSource].axis.x);
            float directionDelta = direction - directionLast;
            if (Mathf.Abs(directionDelta) < Mathf.PI && Mathf.Abs(directionDelta) > directionTolerance && targetSelectAction[inputSource].state != true)
            {
                xrLineVisual.reticle.transform.Rotate(new Vector3(0, directionDelta * turnMultiplier, 0));
            }
            directionLast = direction;
        }
    }

    private void confirmTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {

        if (OnConfirm != null)
        {
            OnConfirm();
        }
    }

    private void turnLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnTurnLeft != null)
        {
            OnTurnLeft();
        }
    }
    private void turnRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnTurnRight != null)
        {
            OnTurnRight();
        }
    }

    private void nudgeForward(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnNudgeForward != null)
        {
            OnNudgeForward();
        }
    }

    private void nudgeBackward(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnNudgeBackward != null)
        {
            OnNudgeBackward();
        }
    }

    private void strafeTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        turnRobot(axis);
        turnCamera(axis);
        moveRobotForwardBack(axis);
    }

    private void turnCamera(Vector2 axis)
    {
        if (Mathf.Abs(axis.x) > 0.5)
        {
            float turnSpeed = 30.0f;
            panTiltPublisher.panOffset += axis.x * turnSpeed * Time.deltaTime;
        }
    }

    private void moveRobotForwardBack(Vector2 axis)
    {
        if (Mathf.Abs(axis.y) > 0.5)
        {
            float moveSpeed = 0.3f;
            Vector3 translation = new Vector3(0, 0, axis.y * moveSpeed * Time.deltaTime);
            xrLineVisual.reticle.transform.Translate(translation, Space.Self);
        }
    }



    private void turnRobot(Vector2 axis)
    {
        if (Mathf.Abs(axis.x) > 0.5)
        {
            float turnSpeed = 30.0f;
            xrLineVisual.reticle.transform.Rotate(new Vector3(0, axis.x * turnSpeed * Time.deltaTime, 0));
        }
    }

    private void stopRobot(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnStop != null)
        {
            OnStop();
        }
    }
}
