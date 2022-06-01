using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeActionSetListener : MonoBehaviour
{
    public ChangeActionSet changeActionSetEvent;
    public UnityEvent onEventTriggered;
    void OnEnable()
    {

        changeActionSetEvent.AddListener(this);
    }
    void OnDisable()
    {
        changeActionSetEvent.RemoveListener(this);
    }
    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();
        Debug.Log(changeActionSetEvent.controlSet);
    }

}
