using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class ParticipantHandler : MonoBehaviour
{ 
    public enum ControlMethod
    {
        Direct,
        Waypoint
    }

    public enum TeleopInterface
    {
        VR,
        Screen
    }

    public enum Delay
    {
        NoDelay,
        Delay,
        LongDelay
    }

    public int participantID;
    public TeleopInterface teleopInterface;

    [Header("Condition")]
    public ControlMethod controlMethod;
    public Delay delayCondition;
    public bool tutorial;

    float delayLevel = 0.5f;
    float delayLevelLong = 1.0f;

    [Header("Do Not Touch")]
    public float delay = 0;

    void OnValidate()
    {
        switch (delayCondition)
        {
            case Delay.NoDelay:
                delay = 0;
                break;
            case Delay.Delay:
                delay = delayLevel;
                break;
            case Delay.LongDelay:
                delay = delayLevelLong;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
