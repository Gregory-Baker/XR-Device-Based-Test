using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchInput : MonoBehaviour
{
    public SteamVR_ActionSet baseActionSet;
    public SteamVR_ActionSet armActionSet;

    public List<GameObject> baseGameObjects = new List<GameObject>();
    public List<GameObject> armGameObjects = new List<GameObject>();

    public void LoadBaseInput()
    {
        baseActionSet.Activate();
        armActionSet.Deactivate();

        foreach (GameObject obj in baseGameObjects)
            obj.SetActive(true);

        foreach (GameObject obj in armGameObjects)
            obj.SetActive(false);
    }

    public void LoadArmInput()
    {
        armActionSet.Activate();
        baseActionSet.Deactivate();

        foreach (GameObject obj in baseGameObjects)
            obj.SetActive(false);

        foreach (GameObject obj in armGameObjects)
            obj.SetActive(true);
    }
}
