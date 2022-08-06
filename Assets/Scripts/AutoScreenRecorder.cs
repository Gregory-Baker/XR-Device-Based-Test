using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using System;

public class AutoScreenRecorder : MonoBehaviour
{
    public ParticipantHandler participantHandler;

    string participantID;
    string control;
    string delay;

    [SerializeField]
    private bool recording = false;

    RecorderControllerSettings controllerSettings;
    RecorderController TestRecorderController;

    // Start is called before the first frame update
    void Start()
    {
        participantID = participantHandler.participantID.ToString();
        control = participantHandler.controlMethod.ToString();
        delay = participantHandler.delayCondition.ToString();

        if (participantHandler.participantID != 0 && !participantHandler.tutorial)
        {
            controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            TestRecorderController = new RecorderController(controllerSettings);

            var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
            videoRecorder.name = "Experiment Recorder";
            videoRecorder.Enabled = true;
            videoRecorder.VideoBitRateMode = UnityEditor.VideoBitrateMode.High;

            videoRecorder.ImageInputSettings = new CameraInputSettings
            {
                Source = ImageSource.MainCamera,
                FlipFinalOutput = true,
                OutputWidth = 1280,
                OutputHeight = 720
            };

            string folder = "C:/Users/g-baker-admin/Documents/MR_Waypoint_Experiment_Data/";
            string datetime = DateTime.Now.Date.Year + "_" + DateTime.Now.Date.Month + "_" + DateTime.Now.Date.Day + "-" + DateTime.Now.TimeOfDay.Hours + "_" + DateTime.Now.TimeOfDay.Minutes + "_" + DateTime.Now.TimeOfDay.Seconds;
            string outfile = folder + participantID + "/" + participantID + "_" + control + "_" + delay + "_" + datetime;


            videoRecorder.AudioInputSettings.PreserveAudio = false;
            videoRecorder.OutputFile = outfile;

            controllerSettings.AddRecorderSettings(videoRecorder);
            controllerSettings.SetRecordModeToManual();
            controllerSettings.FrameRate = 60;
            controllerSettings.CapFrameRate = false;

            RecorderOptions.VerboseMode = false;
            TestRecorderController.PrepareRecording();
            TestRecorderController.StartRecording();
            recording = true;
            Debug.Log("Outfile: " + outfile);
        }


    }

    void OnApplicationQuit()
    {
        if (recording)
        {
            TestRecorderController.StopRecording();
            recording = false;
            Debug.Log("Stopped Recording");
        }

    }
}
