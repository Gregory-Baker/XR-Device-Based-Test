using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ControlModeEvent : UnityEvent<ChangeArmTargetControl.ControlMode>
{
}

public class ChangeArmTargetControlListener : MonoBehaviour
{

    public ChangeArmTargetControl changeArmTargetControl;

    public ControlModeEvent changeArmTargetControlEvents;
    void OnEnable()
    {
        changeArmTargetControl.AddListener(this);
    }
    void OnDisable()
    {
        changeArmTargetControl.RemoveListener(this);
    }
    public void OnEventTriggered(ChangeArmTargetControl.ControlMode controlMode)
    {
        changeArmTargetControlEvents.Invoke(controlMode);
    }
}
