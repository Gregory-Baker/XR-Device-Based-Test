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
    }

    public ControlMode controlMode;

    public void ChangeControlMode()
    {
        controlMode = (controlMode != ControlMode.SpecialFunctions) ? controlMode + 1 : 0;
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
