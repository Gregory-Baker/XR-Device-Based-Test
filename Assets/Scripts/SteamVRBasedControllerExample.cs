using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR;

public class SteamVRBasedControllerExample : XRBaseController
{

    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Pose poseAction = null;

    [Header("SteamVR Input")]
    public SteamVR_Action_Boolean selectAction = null;
    public SteamVR_Action_Boolean activateAction = null;
    public SteamVR_Action_Boolean interfaceAction = null;
    public SteamVR_Action_Boolean targetSelectAction = null;

    public GameObject xrRayObject = null;
    private XRRayInteractor xrRayInteractor = null;
    private XRInteractorLineVisual xrLineVisual = null;

    public SteamVR_Action_Vector2 targetRotation = null;
    public float directionTolerance = 0.01f;
    public float turnMultiplier = 20;
    float directionLast = 0;

    public SteamVR_Action_Boolean targetConfirmAction = null;
    public delegate void ConfirmAction();
    public static event ConfirmAction OnConfirm;

    public SteamVR_Action_Boolean turnLeftAction = null;
    public delegate void TurnLeftAction();
    public static event TurnLeftAction OnTurnLeft;

    public SteamVR_Action_Boolean turnRightAction = null;
    public delegate void TurnRightAction();
    public static event TurnLeftAction OnTurnRight;

    public SteamVR_Action_Boolean stopAction = null;
    public delegate void StopAction();
    public static event TurnLeftAction OnStop;

    // Start is called before the first frame update
    void Start()
    {
        SteamVR.Initialize();

        xrRayInteractor = xrRayObject.GetComponent<XRRayInteractor>();
        xrLineVisual = xrRayObject.GetComponent<XRInteractorLineVisual>();
    }


    protected override void Awake()
    {
        base.Awake();

        targetSelectAction[inputSource].onStateDown += enableTargetSelection;
        targetSelectAction[inputSource].onStateUp += disableTargetSelection;
        targetRotation[inputSource].onAxis += turnTargetToAxisDirection;
        targetConfirmAction[inputSource].onStateDown += confirmTarget;
        turnLeftAction[inputSource].onStateDown += turnLeft;
        turnRightAction[inputSource].onStateDown += turnRight;
        stopAction[inputSource].onStateDown += stopRobot;
    }


    private void OnDestroy()
    {
        targetSelectAction[inputSource].onStateDown -= enableTargetSelection;
        targetSelectAction[inputSource].onStateDown -= disableTargetSelection;
        targetRotation[inputSource].onAxis -= turnTargetToAxisDirection;
        targetConfirmAction[inputSource].onStateDown -= confirmTarget;
        turnLeftAction[inputSource].onStateDown -= turnLeft;
        turnRightAction[inputSource].onStateDown -= turnRight;
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

    private void turnTargetToAxisDirection(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (targetRotation != null && targetRotation[inputSource].axis.magnitude > 0)
        {
            float direction = -Mathf.Atan2(targetRotation[inputSource].axis.y, targetRotation[inputSource].axis.x);
            // targetSelectionObject.reticle.transform.localEulerAngles = new Vector3(0, Mathf.Rad2Deg * direction, 0);
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

    private void stopRobot(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnStop != null)
        {
            OnStop();
        }
    }




    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        if (controllerState != null)
        {

            controllerState.inputTrackingState = InputTrackingState.All;

            Vector3 position = poseAction[inputSource].localPosition;
            controllerState.position = position;

            Quaternion rotation = poseAction[inputSource].localRotation;
            controllerState.rotation = rotation;
        }
    }


    protected override void UpdateInput(XRControllerState controllerState)
    {
        base.UpdateInput(controllerState);
        if (controllerState == null)
            return;

        controllerState.ResetFrameDependentStates();

        controllerState.selectInteractionState.SetFrameState(selectAction[inputSource].state);
        controllerState.activateInteractionState.SetFrameState(activateAction[inputSource].state);
        controllerState.uiPressInteractionState.SetFrameState(interfaceAction[inputSource].state);

    }

    private void SetInteractionState(ref InteractionState interactionState, SteamVR_Action_Boolean_Source action)
    {

        interactionState.activatedThisFrame = action.stateDown;
        interactionState.deactivatedThisFrame = action.stateUp;
        interactionState.SetFrameState(action.state);
    }

}
