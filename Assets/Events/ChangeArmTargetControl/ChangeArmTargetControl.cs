using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Events/Change Arm Target Control Mode")]
public class ChangeArmTargetControl : ScriptableObject
{
    public enum ControlMode
    {
        TargetPositionControl,
        SpecialFunctions,
        ConfirmActionSuccess
    }

    public ControlMode controlMode;

    public void ChangeControlMode()
    {
        switch (controlMode)
        {
            case ControlMode.TargetPositionControl:
                controlMode = ControlMode.SpecialFunctions;
                break;
            case ControlMode.SpecialFunctions:
                controlMode = ControlMode.TargetPositionControl;
                break;
            case ControlMode.ConfirmActionSuccess:
                controlMode = ControlMode.SpecialFunctions;
                break;
        }
        TriggerEvent();
    }

    public void SetControlMode(ControlMode control)
    {
        controlMode = control;
        TriggerEvent();
    }

    private List<ChangeArmTargetControlListener> listeners = new List<ChangeArmTargetControlListener>();
    public void TriggerEvent()
    {
        // ChangeControlMode();

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered(controlMode);
        }
    }
    public void AddListener(ChangeArmTargetControlListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(ChangeArmTargetControlListener listener)
    {
        listeners.Remove(listener);
    }
}
