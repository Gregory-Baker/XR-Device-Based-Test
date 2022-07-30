using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessIconHandler : MonoBehaviour
{
    public GameObject successIcon;


    public void ToggleIcon(ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.ConfirmActionSuccess:
                successIcon.SetActive(true);
                break;
            default:
                successIcon.SetActive(false);
                break;

        }
    }
}
