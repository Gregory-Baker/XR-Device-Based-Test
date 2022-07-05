using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectVisibility : MonoBehaviour
{

    public bool hidden = false;

    Renderer[] objectRenderers;

    void Awake()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();
    }

    public void HideObjectAndChildren()
    {
        if (!hidden)
        {
            foreach (Renderer renderer in objectRenderers)
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
            foreach (Renderer renderer in objectRenderers)
            {
                renderer.enabled = true;
            }
            hidden = false;
        }
    }
}
