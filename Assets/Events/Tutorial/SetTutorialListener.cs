using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class SetTutorialEvent : UnityEvent<SetTutorial.Tutorial>
{
}

public class SetTutorialListener : MonoBehaviour
{
    public SetTutorial setTutorial;

    public SetTutorialEvent setTutorialEvents;
    void OnEnable()
    {
        setTutorial.AddListener(this);
    }
    void OnDisable()
    {
        setTutorial.RemoveListener(this);
    }
    public void OnEventTriggered(SetTutorial.Tutorial tutorial)
    {
        setTutorialEvents.Invoke(tutorial);
    }
}