using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Events/Load Base Controller")]
public class LoadBaseController : ScriptableObject
{

    private List<LoadBaseControllerListener> listeners = new List<LoadBaseControllerListener>();
    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }
    public void AddListener(LoadBaseControllerListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(LoadBaseControllerListener listener)
    {
        listeners.Remove(listener);
    }
}
