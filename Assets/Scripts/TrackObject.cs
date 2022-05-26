using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackObject : MonoBehaviour
{
    [SerializeField]
    GameObject objectToTrack;

    [SerializeField]
    string objectName;

    [SerializeField]
    Vector3 offset = new Vector3(0, 0, 0);

    [SerializeField]
    ZEDManager zed = null;

    private void OnEnable()
    {
        zed.OnZEDReady += SetTrackingObject;
    }

    private void OnDisable()
    {
        zed.OnZEDReady -= SetTrackingObject;
    }

    private void SetTrackingObject()
    {
        if (objectToTrack == null)
        {
            objectToTrack = GameObject.Find(objectName);
            //transform.SetParent(objectToTrack.transform);
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToTrack != null)
        {
            Vector3 positionDifference = objectToTrack.transform.position - transform.position;

            transform.Translate(positionDifference);
            transform.Translate(offset);
        }
    }
}
