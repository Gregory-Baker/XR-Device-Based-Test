using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticipantHandler : MonoBehaviour
{ 
    public enum Condition
    {
        Direct,
        Waypoint,
        DirectDelay,
        WaypointDelay
    }

    [Header("Condition")]
    public int participantID;

    public Condition condition;


    [Header("Read Only Settings")]
    public float delay = 0;

    [Header("Variables")]
    [SerializeField]
    float delayValue = 1;


    // Update is called once per frame
    void Update()
    {
        SetDelay();
    }

    private void SetDelay()
    {
        switch (condition)
        {
            case Condition.DirectDelay:
                delay = delayValue;
                break;
            case Condition.WaypointDelay:
                delay = delayValue;
                break;
            default:
                delay = 0;
                break;
                
        }
            
    }
}
