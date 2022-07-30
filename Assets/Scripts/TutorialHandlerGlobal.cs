using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandlerGlobal : MonoBehaviour
{
    public ParticipantHandler participantHandler;

    public GameObject[] tutorialOnlyObjects;

    public SetTutorial setTutorial;

    public SetTutorial.Tutorial tutorial;

    // Start is called before the first frame update
    void OnValidate()
    {
        setTutorial.Set(tutorial);
        setTutorial.TriggerEvent();
    }

    // Update is called once per frame
    void Start()
    {
        if (!participantHandler.tutorial)
        {
            foreach (var gameObject in tutorialOnlyObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
