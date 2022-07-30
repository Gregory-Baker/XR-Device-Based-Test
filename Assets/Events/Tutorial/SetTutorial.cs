using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Events/Set Tutorial")]
public class SetTutorial : ScriptableObject
{
    public enum Tutorial
    {
        None,
        TargetSetting,
        Movement,
        Pick,
        Place
    }

    public Tutorial tutorial;

    public void Set(Tutorial setTutorial)
    {
        tutorial = setTutorial;
        TriggerEvent();
    }

    private List<SetTutorialListener> listeners = new List<SetTutorialListener>();
    public void TriggerEvent()
    {

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered(tutorial);
        }
    }
    public void AddListener(SetTutorialListener listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(SetTutorialListener listener)
    {
        listeners.Remove(listener);
    }
}
