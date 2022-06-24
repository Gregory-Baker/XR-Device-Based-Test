using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    [Header("Actions")]
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Boolean touch = null;
    public SteamVR_Action_Boolean press = null;
    public SteamVR_Action_Vector2 touchPosition = null;

    [Header("Scene Objects")]
    public RadialMenu radialMenu = null;

    private void Awake()
    {
        touch[inputSource].onChange += OnTouch;
        press[inputSource].onStateUp += OnPressReleased;
        touchPosition[inputSource].onAxis += TouchPosition;
    }


    private void OnDestroy()
    {
        touch[inputSource].onChange -= OnTouch;
        press[inputSource].onStateUp -= OnPressReleased;
        touchPosition[inputSource].onAxis -= TouchPosition;
    }


    private void OnTouch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        radialMenu.Show(newState);
    }

    private void OnPressReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        radialMenu.ActivateHighlightedSection();
    }

    private void TouchPosition(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        radialMenu.SetTouchPositon(axis);
    }


}
