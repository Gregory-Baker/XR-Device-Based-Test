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
    float headingThreshold = 10f;

    MeshRenderer[] objectRenderers;

    void Start ()
    {
        objectRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(SetTrackingObject());
    }

    IEnumerator SetTrackingObject()
    {
        while (targetObject == null)
        {
            targetObject = GameObject.Find(objectName);
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void HideObjectAndChildren()
    {
        foreach (MeshRenderer renderer in objectRenderers)
        {
            renderer.enabled = false;
        }
    }


    public void ShowObjectAndChildren()
    {
        foreach (MeshRenderer renderer in objectRenderers)
        {
            renderer.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (ActionSetHandler.controlSet == ActionSetHandler.CurrentControlSet.Base && targetObject != null)
        {
            float distanceToObject = Vector3.Distance(transform.position, targetObject.transform.position);
            float headingDifference = Mathf.Abs(transform.rotation.eulerAngles.y - targetObject.transform.rotation.eulerAngles.y);

            if ((distanceToObject < distanceThreshold) && (headingDifference < headingThreshold))
            {

                HideObjectAndChildren();
            }
            else
            {
                ShowObjectAndChildren();
            }
        }
    }
}
