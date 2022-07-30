using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialVideoHandler : MonoBehaviour
{
    VideoPlayer videoPlayer;

    public VideoClip[] videoClips;

    private int currentClip = -1;

    public GameObject playButton = null;

    // Start is called before the first frame update
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += EndReached;
    }

    private void EndReached(VideoPlayer source)
    {
        videoPlayer.enabled = false;
    }

    public void SetTutorialVideo(SetTutorial.Tutorial tutorial)
    {
        videoPlayer.enabled = true;

        switch (tutorial)
        {
            case SetTutorial.Tutorial.None:
                currentClip = -1;
                videoPlayer.enabled = false;
                playButton.SetActive(false);
                break;
            case SetTutorial.Tutorial.TargetSetting:
                currentClip = 0;
                videoPlayer.clip = videoClips[currentClip];
                break;
            case SetTutorial.Tutorial.Movement:
                currentClip = 1;
                videoPlayer.clip = videoClips[currentClip];
                break;
            case SetTutorial.Tutorial.Pick:
                currentClip = 2;
                videoPlayer.clip = videoClips[currentClip];
                break;
            case SetTutorial.Tutorial.Place:
                currentClip = 3;
                videoPlayer.clip = videoClips[currentClip];
                break;

        }
    }

    public void SetTutorialVideo(int clipIndex)
    {
        if (clipIndex < videoClips.Length)
        {
            videoPlayer.clip = videoClips[clipIndex];
        }
        else
        {
            videoPlayer.enabled = false;
        }
    }

    public void PlayTutorial()
    {
        if (videoPlayer.enabled)
            videoPlayer.Play();
    }

    public void PlayNextTutorial()
    {
        currentClip += 1;
        if (currentClip < videoClips.Length)
        {
            SetTutorialVideo(currentClip);
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.enabled = false;
            if (playButton != null)
            {
                playButton.SetActive(false);
            }
        }
    }

    public void SetAndPlayTutorial(SetTutorial.Tutorial tutorial)
    {
        SetTutorialVideo(tutorial);
        PlayTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
