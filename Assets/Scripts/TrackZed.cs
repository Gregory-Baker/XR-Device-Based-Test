using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackZed : MonoBehaviour
{
    [SerializeField]
    GameObject objectToTrack = null;

    public bool position = true;
    public bool rotation = true;

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
            //transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (objectToTrack != null)
        {   
            if (position && rotation)
            {
                transform.SetPositionAndRotation(objectToTrack.transform.position + offset, objectToTrack.transform.rotation);
            } 
            else if (position) 
            {
                transform.position = objectToTrack.transform.position + offset;
            } 
            else if (rotation)
            {
                transform.rotation = objectToTrack.transform.rotation;
            }
            
        }
    }
}
