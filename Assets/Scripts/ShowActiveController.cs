using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowActiveController : MonoBehaviour
{
    [SerializeField]
    Texture armControlIcon;

    [SerializeField]
    Texture baseControlIcon;

    RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }


    public void ChangeIcon(ChangeActionSet changeActionSetEvent)
    {
        switch (changeActionSetEvent.controlSet)
        {
            case ChangeActionSet.CurrentControlSet.BASE:
                rawImage.texture = baseControlIcon;
                break;
            case ChangeActionSet.CurrentControlSet.ARM:
                rawImage.texture = armControlIcon;
                break;
            default:
                Debug.Log("Controller set to neither arm or base - something wrong here");
                break;

        }

    }

}
