using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Events/Change Action Set")]
public class ChangeActionSet : ScriptableObject
{
    public enum CurrentControlSet
    {
        BASE,
        ARM
    }

    public CurrentControlSet controlSet = CurrentControlSet.BASE;
    private List<ChangeActionSetListener> listeners = new List<ChangeActionSetListener>();
    public void TriggerEvent()
    {
        SwitchControlSet();
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }
    public void AddListener(ChangeActionSetListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(ChangeActionSetListener listener)
    {
        listeners.Remove(listener);
    }

    public void SwitchControlSet()
    {
        switch (controlSet)
        {
            case CurrentControlSet.BASE:
                controlSet = CurrentControlSet.ARM;
                break;
            case CurrentControlSet.ARM:
                controlSet = CurrentControlSet.BASE;
                break;
            default:
                Debug.Log("Controller set to neither arm or base - something wrong here");
                break;

        }
    }
}
