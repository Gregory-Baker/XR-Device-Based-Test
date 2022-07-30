using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchInput : MonoBehaviour
{
    KeyboardTeleop inputActions;

    public SteamVR_ActionSet baseActionSet;
    public SteamVR_ActionSet armActionSet;

    public List<GameObject> baseGameObjects = new List<GameObject>();
    public List<GameObject> armGameObjects = new List<GameObject>();

    private void Awake()
    {
        inputActions = new KeyboardTeleop();
    }

    public void LoadBaseInput()
    {
        baseActionSet.Activate();
        armActionSet.Deactivate();

        //inputActions.Keyboard.Enable();
        //inputActions.Arm.Disable();


        foreach (GameObject obj in baseGameObjects)
            obj.SetActive(true);

        foreach (GameObject obj in armGameObjects)
            obj.SetActive(false);
    }

    public void LoadArmInput()
    {
        armActionSet.Activate();
        baseActionSet.Deactivate();

        //inputActions.Keyboard.Disable();
        //inputActions.Arm.Enable();

        foreach (GameObject obj in baseGameObjects)
            obj.SetActive(false);

        foreach (GameObject obj in armGameObjects)
            obj.SetActive(true);
    }
}
