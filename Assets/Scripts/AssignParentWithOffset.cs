using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssignParentWithOffset : MonoBehaviour
{
    [SerializeField]
    protected GameObject targetObject;

    [SerializeField]
    protected string objectName;

    [SerializeField]
    Vector3 positionOffset = Vector3.zero;

    [SerializeField]
    Vector3 rotationOffset = Vector3.zero;


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

        SetParentAndPosition();

    }

    public void SetParentAndPosition()
    {
        if(targetObject != null)
        {
            transform.SetParent(targetObject.transform);
            transform.localPosition = positionOffset;
            transform.localEulerAngles = rotationOffset;
            //transform.Translate(positionOffset, Space.Self);
            //transform.Rotate(transform.up, rotationOffset);
        }
    }

}
