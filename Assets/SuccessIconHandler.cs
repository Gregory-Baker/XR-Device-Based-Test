using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessIconHandler : MonoBehaviour
{
    RawImage rawImage;

    public Texture successIcon;
    public Texture emptyIcon;

    private void Awake()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();
    }

    public void ToggleIcon(ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.ConfirmActionSuccess:
                rawImage.texture = successIcon;
                break;
            default:
                rawImage.texture = emptyIcon;
                break;

        }
    }
}
