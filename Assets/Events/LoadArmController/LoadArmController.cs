using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Events/Load Arm Controller")]
public class LoadArmController : ScriptableObject
{

    private List<LoadArmControllerListener> listeners = new List<LoadArmControllerListener>();
    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }
    public void AddListener(LoadArmControllerListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(LoadArmControllerListener listener)
    {
        listeners.Remove(listener);
    }
}
