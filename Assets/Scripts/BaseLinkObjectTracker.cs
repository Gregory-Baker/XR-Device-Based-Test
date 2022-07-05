using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLinkObjectTracker : MonoBehaviour
{
    GameObject baseLinkObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetBaseLinkObject());
    }

    private IEnumerator SetBaseLinkObject()
    {
        while (baseLinkObject == null)
        {
            baseLinkObject = GameObject.Find("base_link");
            yield return null;
        }
        transform.SetParent(baseLinkObject.transform);
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
