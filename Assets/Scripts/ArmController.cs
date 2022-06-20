using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEngine.Events;

public class ArmController : MonoBehaviour
{
    [Header("SteamVR Tracking")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    public GameObject targetObject;

    [Header("SteamVR Input")]
    public SteamVR_Action_Vector2 targetRotationAction = null;
    public float directionTolerance = 0.01f;
    public float turnAngle = 1f;
    float directionLast = 0;

    public SteamVR_Action_Boolean increaseTargetDistanceAction = null;
    public SteamVR_Action_Boolean decreaseTargetDistanceAction = null;

    public float minTargetDistance = 0.1f;
    public float maxTargetDistance = 1.5f;
    public float moveSpeed = 0.1f;

    public SteamVR_Action_Boolean targetConfirmAction = null;
    public UnityEvent confirmTargetEvents;

    private void OnEnable()
    {
        targetRotationAction[inputSource].onAxis += turnTarget;
        increaseTargetDistanceAction[inputSource].onState += increaseTargetDistance;
        decreaseTargetDistanceAction[inputSource].onState += decreaseTargetDistance;
        targetConfirmAction[inputSource].onStateDown += confirmTarget;
    }

    private void OnDestroy()
    {
        targetRotationAction[inputSource].onAxis -= turnTarget;
        increaseTargetDistanceAction[inputSource].onState -= increaseTargetDistance;
        decreaseTargetDistanceAction[inputSource].onState -= decreaseTargetDistance;
        targetConfirmAction[inputSource].onStateDown -= confirmTarget;
    }

    private void increaseTargetDistance(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Vector3 vectorToTarget = targetObject.transform.position - transform.position;
        if(vectorToTarget.magnitude < maxTargetDistance)
        {
            targetObject.transform.Translate(vectorToTarget.normalized * moveSpeed, Space.World);
        }
        
    }


    private void decreaseTargetDistance(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Vector3 vectorToTarget = targetObject.transform.position - transform.position;
        if (vectorToTarget.magnitude > minTargetDistance)
        {
            targetObject.transform.Translate(vectorToTarget.normalized * -moveSpeed, Space.World);
        }
    }

    private void turnTarget(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        if (axis.magnitude > 0.1)
        {
            float direction = -Mathf.Atan2(targetRotationAction[inputSource].axis.y, targetRotationAction[inputSource].axis.x);
            float directionDelta = direction - directionLast;
            if (Mathf.Abs(directionDelta) < Mathf.PI && Mathf.Abs(directionDelta) > directionTolerance)
            {
                targetObject.transform.Rotate(new Vector3(0, Mathf.Sign(directionDelta) * turnAngle / Time.deltaTime, 0));
            }
            directionLast = direction;
        }
    }


    private void confirmTarget(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        confirmTargetEvents.Invoke();
    }


}