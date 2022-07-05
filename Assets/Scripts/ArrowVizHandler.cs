using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowVizHandler : MonoBehaviour
{
    public GameObject upRotateArrows;
    public GameObject fblrArrows;

    public void ShowArrows(ChangeArmTargetControl.ControlMode controlMode)
    {
        switch (controlMode)
        {
            case ChangeArmTargetControl.ControlMode.SpecialFunctions:
                upRotateArrows.SetActive(true);
                fblrArrows.SetActive(false);
                break;
            case ChangeArmTargetControl.ControlMode.TargetPositionControl:
                upRotateArrows.SetActive(false);
                fblrArrows.SetActive(true);
                break;
        }
    }
}
