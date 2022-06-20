using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectVisibility : MonoBehaviour
{

    public bool hidden = false;

    MeshRenderer[] objectRenderers;

    void Awake()
    {
        objectRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void HideObjectAndChildren()
    {
        if (!hidden)
        {
            foreach (MeshRenderer renderer in objectRenderers)
            {
                renderer.enabled = false;
            }
            hidden = true;
        }
    }


    public void ShowObjectAndChildren()
    {
        if (hidden)
        {
            foreach (MeshRenderer renderer in objectRenderers)
            {
                renderer.enabled = true;
            }
            hidden = false;
        }
    }
}
