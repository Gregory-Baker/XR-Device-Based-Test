using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadArmControllerListener : MonoBehaviour
{
    public LoadArmController LoadArmController;
    public UnityEvent LoadArmControllerEvents;
    void OnEnable()
    {
        LoadArmController.AddListener(this);
    }
    void OnDisable()
    {
        LoadArmController.RemoveListener(this);
    }
    public void OnEventTriggered()
    {
        LoadArmControllerEvents.Invoke();
    }
}
