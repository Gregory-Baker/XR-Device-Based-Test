using BioIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandler : MonoBehaviour
{
    public IKSolver ikSolver;

    public GameObject targetObject;

    public float distanceThreshold = 0.01f;
    public float headingThreshold = 1f;
    public float timeThreshold = 5f;

    Vector3 prevPos = Vector3.zero;
    float prevHeading = 0f;

    float enabledTime;
        
    // Start is called before the first frame update
    void Start()
    {
        enabledTime = Time.time;
        ikSolver.gameObject.SetActive(false);
        prevPos = targetObject.transform.position;
        prevHeading = targetObject.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        float posChange = Vector3.Distance(targetObject.transform.position, prevPos);
        float headingChange = Mathf.Abs(prevHeading - targetObject.transform.rotation.eulerAngles.y);


        if (posChange > distanceThreshold || headingChange > headingThreshold)
        {
            prevPos = targetObject.transform.position;
            prevHeading = targetObject.transform.rotation.eulerAngles.y;
            enabledTime = Time.time;
            if (!ikSolver.gameObject.activeSelf)
            {
                ikSolver.gameObject.SetActive(true);
                Debug.Log("IK SOLVER ENABLED");
            }
        }
        else if (ikSolver.gameObject.activeSelf && (Time.time - enabledTime) > timeThreshold)
        {
            ikSolver.gameObject.SetActive(false);
            Debug.Log("IK SOLVER DISABLED");
        }
    }

}
