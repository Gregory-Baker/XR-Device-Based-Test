using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackObject : MonoBehaviour
{
    [SerializeField]
    GameObject objectToTrack = null;

    [SerializeField]
    string objectName;

    [SerializeField]
    Vector3 positionOffset = Vector3.zero;

    [SerializeField]
    float rotationOffset = 0f;

    [SerializeField]
    bool moveEveryFrame = true;

    private void OnEnable()
    {
        StartCoroutine(SetTrackingObject());
    }

    IEnumerator SetTrackingObject()
    {
        while (objectToTrack == null)
        {
            objectToTrack = GameObject.Find(objectName);
            yield return new WaitForSeconds(0.5f);
        }
        MoveToTarget();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (moveEveryFrame)
        {
            MoveToTarget();
        }
    }

    public void MoveToTarget()
    {
        if (objectToTrack != null)
        {
            transform.SetPositionAndRotation(objectToTrack.transform.position, objectToTrack.transform.rotation);
            transform.Translate(positionOffset, Space.Self);
            transform.Rotate(transform.up, rotationOffset);
        }
    }
}
