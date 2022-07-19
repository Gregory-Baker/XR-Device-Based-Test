using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEditor.PackageManager;
using UnityEngine.InputSystem;

public class BaseController : MonoBehaviour
{
    KeyboardTeleop inputActions;

    [Header("SteamVR Input Source")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    private XRRayInteractor xrRayInteractor = null;
    private XRInteractorLineVisual xrLineVisual = null;

    [Header("SteamVR Actions")]
    public SteamVR_Action_Boolean targetSelectAction = null;
    public SteamVR_Action_Vector2 targetRotation = null;

    public SteamVR_Action_Vector2 strafeTargetAction = null;

    public SteamVR_Action_Boolean targetConfirmAction = null;
    public delegate void ConfirmAction();
    public static event ConfirmAction OnConfirm;


    public SteamVR_Action_Boolean turnCamLeftAction = null;
    public delegate void TurnCamLeft();
    public static event TurnCamLeft OnTurnCamLeft;

    public SteamVR_Action_Boolean turnCamRightAction = null;
    public delegate void TurnCamRight();
    public static event TurnCamRight OnTurnCamRight;

    public delegate void TiltCamUp();
    public static event TiltCamUp OnTiltCamUp;

    public delegate void TiltCamDown();
    public static event TiltCamDown OnTiltCamDown;

    public delegate void TurnRobotToCamAction();
    public static event TurnRobotToCamAction OnTurnRobotToCam;

    public SteamVR_Action_Boolean moveForwardAction = null;
    public delegate void MoveForward();
    public static event MoveForward OnMoveForward;

    public SteamVR_Action_Boolean moveBackwardAction = null;
    public delegate void MoveBackward();
    public static event MoveBackward OnMoveBackward;

    public delegate void MoveRobotToTargetDelegate (int direction);
    public static event MoveRobotToTargetDelegate OnMoveRobotToTarget;

    public delegate void MoveTargetoRobotDelegate();
    public static event MoveTargetoRobotDelegate OnMoveTargetToRobot;


    public SteamVR_Action_Boolean stopAction = null;
    public delegate void StopAction();
    public static event StopAction OnStop;

    [Header("Configurable Parameters")]
    public float targetTurnMultiplier = 120f;
    

    [Header("External Objects")]
    // public GameObject baseLinkObject = null;
    public GameObject xrRayObject = null;
    public RosHeadRotationPublisher panTiltPublisher;

    // Internal Parameters
    float directionLast = 0;
    public bool robotStopCommanded = false;
    int moveDirection = 1;

    private void Awake()
    {
        inputActions = new KeyboardTeleop();

        inputActions.FindAction("GoToGoal").started += confirmTargetKeyboard;

        inputActions.FindAction("MoveRobotForward").started += strafeTargetKeyboard;
        inputActions.FindAction("MoveRobotForward").canceled += strafeTargetKeyboardUp;
        inputActions.FindAction("TurnRobot").started += turnRobotKeyboard;
        inputActions.FindAction("TurnRobot").canceled += turnRobotKeyboardUp;

        inputActions.FindAction("StopRobot").started += stopRobot;

        inputActions.FindAction("TurnCam").started += turnRobotKeyboard;

        inputActions.FindAction("TiltCam").started += tiltCamKeyboard;

    }

    // Start is called before the first frame update
    void OnEnable()
    {
        inputActions.Enable();

        xrRayInteractor = xrRayObject.GetComponent<XRRayInteractor>();
        xrLineVisual = xrRayObject.GetComponent<XRInteractorLineVisual>();

        targetSelectAction[inputSource].onStateDown += enableTargetSelection;
        targetSelectAction[inputSource].onStateUp += disableTargetSelection;

        targetRotation[inputSource].onAxis += turnTarget;
        strafeTargetAction[inputSource].onAxis += strafeTarget;
        targetConfirmAction[inputSource].onStateDown += confirmTarget;

        turnCamLeftAction[inputSource].onStateDown += MoveTargetToRobot;
        turnCamLeftAction[inputSource].onState += turnCamLeft;
        turnCamLeftAction[inputSource].onStateUp += TurnRobotToCamera;

        turnCamRightAction[inputSource].onStateDown += MoveTargetToRobot;
        turnCamRightAction[inputSource].onStateUp += TurnRobotToCamera;
        turnCamRightAction[inputSource].onState += turnCamRight;

        moveForwardAction[inputSource].onStateDown += MoveTargetToRobot;
        moveForwardAction[inputSource].onState += MoveTargetForward;
        moveForwardAction[inputSource].onStateUp += MoveRobotToTarget;

        moveBackwardAction[inputSource].onStateDown += MoveTargetToRobot;
        moveBackwardAction[inputSource].onState += MoveTargetBackward;
        moveBackwardAction[inputSource].onStateUp += MoveRobotToTarget;

        stopAction[inputSource].onStateDown += stopRobot;
    }

    // Update is called once per frame
    void OnDisable()
    {
        inputActions.Disable();

    }

    private void strafeTargetKeyboardUp(InputAction.CallbackContext obj)
    {
        if (OnMoveRobotToTarget != null)
        {
            OnMoveRobotToTarget(moveDirection);
        }
    }

    private void strafeTargetKeyboard(InputAction.CallbackContext obj)
    {
        StartCoroutine(strafeTargetKeyboardCoroutine(obj));
    }

    private IEnumerator strafeTargetKeyboardCoroutine(InputAction.CallbackContext obj)
    {
        while (Mathf.Abs(obj.ReadValue<float>()) > 0.1)
        {
            moveDirection = (int)obj.ReadValue<float>();
            if (moveDirection > 0)
            {
                moveDirection = 1;
                if (OnMoveForward != null)
                {
                    OnMoveForward();
                }
            }
            else
            {
                moveDirection = -1;
                if (OnMoveBackward != null)
                {
                    OnMoveBackward();
                }
            }

            yield return null;
        }
    }


    private void turnRobotKeyboard(InputAction.CallbackContext obj)
    {
        StartCoroutine(turnCamKeyboardCoroutine(obj));
    }

    private IEnumerator turnCamKeyboardCoroutine(InputAction.CallbackContext obj)
    {
        while (Mathf.Abs(obj.ReadValue<float>()) > 0.1)
        {
            int turnDirection = (int)obj.ReadValue<float>();
            if (turnDirection < 0)
            {
                if (OnTurnCamLeft != null)
                {
                    OnTurnCamLeft();
                }
            }
            else
            {
                if (OnTurnCamRight != null)
                {
                    OnTurnCamRight();
                }
            }

            yield return null;
        }

    }
    
    private void tiltCamKeyboard(InputAction.CallbackContext obj)
    {
        StartCoroutine(tiltCamKeyboardCoroutine(obj));
    }

    private IEnumerator tiltCamKeyboardCoroutine(InputAction.CallbackContext obj)
    {
        while (Mathf.Abs(obj.ReadValue<float>()) > 0.1)
        {
            int tiltDirection = (int)obj.ReadValue<float>();
            if (tiltDirection < 0)
            {
                if (OnTiltCamDown != null)
                {
                    OnTiltCamDown();
                }
            }
            else
            {
                if (OnTiltCamUp != null)
                {
                    OnTiltCamUp();
                }
            }

            yield return null;
        }

    }

    private void turnRobotKeyboardUp(InputAction.CallbackContext obj)
    {
        if (OnTurnRobotToCam != null)
            OnTurnRobotToCam();
    }

    private void MoveTargetToRobot(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnMoveTargetToRobot != null)
        {
            OnMoveTargetToRobot();
        }
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
            if (Mathf.Abs(directionDelta) < Mathf.PI && targetSelectAction[inputSource].state != true)
            {
                xrLineVisual.reticle.transform.Rotate(new Vector3(0, Mathf.Rad2Deg * directionDelta * targetTurnMultiplier * Time.deltaTime, 0));
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

    private void confirmTargetKeyboard(InputAction.CallbackContext obj)
    {
        if (OnConfirm != null)
        {
            OnConfirm();
        }
    }

    private void turnCamLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnTurnCamLeft != null)
        {
            OnTurnCamLeft();
        }
    }

    private void turnCamRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnTurnCamRight != null)
        {
            OnTurnCamRight();
        }
    }

    private void MoveTargetForward(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        moveDirection = 1;
        if (OnMoveForward != null)
        {
            OnMoveForward();
        }
    }

    private void MoveTargetBackward(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        moveDirection = -1;
        if (OnMoveBackward != null)
        {
            OnMoveBackward();
        }
    }


    private void MoveRobotToTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnMoveRobotToTarget != null)
        {
            OnMoveRobotToTarget(moveDirection);
        }
    }

    private void strafeTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        turnRobot(axis);
        // turnCamera(axis);
        moveRobotForwardBack(axis);
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

    private void TurnRobotToCamera(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (OnTurnRobotToCam != null)
            OnTurnRobotToCam();
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
        robotStopCommanded = true;
        if (OnStop != null)
        {
            OnStop();
        }
    }
    private void stopRobot(InputAction.CallbackContext obj)
    {
        robotStopCommanded = true;
        if (OnStop != null)
        {
            OnStop();
        }
    }
}
