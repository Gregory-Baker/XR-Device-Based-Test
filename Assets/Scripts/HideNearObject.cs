using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNearObject : ChangeObjectVisibility
{
    [SerializeField]
    protected GameObject targetObject;

    [SerializeField]
    protected string objectName;

    [SerializeField]
    protected float distanceThreshold = 0.3f;

    [SerializeField]
    protected float headingThreshold = 10f;

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
