using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZedRecorder : MonoBehaviour
{
    [SerializeField]
    String participantID = "test";
    public ZEDManager zedManager;

    // Start is called before the first frame update
    void Start()
    {
        if(zedManager != null)
        {
            zedManager.OnZEDReady += StartRecording;
        }
    }

    private void StartRecording()
    {
        string folder = "C:/Users/g-baker-admin/Documents/MR_Waypoint_Experiment_Data/";
        string datetime = DateTime.Now.Date.Year + "_" + DateTime.Now.Date.Month + "_" + DateTime.Now.Date.Day + "-" + DateTime.Now.TimeOfDay.Hours + "_" + DateTime.Now.TimeOfDay.Minutes + "_" + DateTime.Now.TimeOfDay.Seconds;
        string outfile = folder + participantID + "/"  + participantID + "_" + datetime + ".svo";
        Debug.Log("Outfile: " + outfile);
        zedManager.zedCamera.EnableRecording(outfile);

    }


}
