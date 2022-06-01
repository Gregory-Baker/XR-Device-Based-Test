using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadBaseControllerListener : MonoBehaviour
{
    public LoadBaseController loadBaseController;
    public UnityEvent loadBaseControllerEvents;
    void OnEnable()
    {
        loadBaseController.AddListener(this);
    }
    void OnDisable()
    {
        loadBaseController.RemoveListener(this);
    }
    public void OnEventTriggered()
    {
        loadBaseControllerEvents.Invoke();
    }
}
