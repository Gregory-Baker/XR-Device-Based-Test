using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNearObject : MonoBehaviour
{
    [SerializeField]
    GameObject targetObject;

    [SerializeField]
    string objectName;

    [SerializeField]
    float distanceThreshold = 0.3f;

    [SerializeField]
    float angleThreshold = 0.3f;
    
    [SerializeField]
    ZEDManager zed;

    MeshRenderer[] objectRenderers;

    void Start ()
    {
        if (zed == null)
        {
            zed = FindObjectOfType<ZEDManager>();
        }

        objectRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        zed.OnZEDReady += AssignTarget;
    }

    private void OnDisable()
    {
        zed.OnZEDReady += AssignTarget;
    }


    private void AssignTarget()
    {
        if (targetObject == null)
        {
            targetObject = GameObject.Find(objectName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            float distanceToObject = (transform.position - targetObject.transform.position).magnitude;
            // float angleToObject = transform.rotation - targetObject.transform.rotation).eulerAngles.magnitude
            if (distanceToObject < distanceThreshold)
            {
                foreach(MeshRenderer renderer in objectRenderers)
                {
                    renderer.enabled = false;
                }
                
            }
            else
            {
                foreach (MeshRenderer renderer in objectRenderers)
                {
                    renderer.enabled = true;
                }
            }
        }
    }
}
