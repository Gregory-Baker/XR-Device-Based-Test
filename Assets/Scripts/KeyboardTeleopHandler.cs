using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardTeleopHandler : MonoBehaviour
{
    public Camera raycastCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = worldPosition;

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = raycastCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, ray.direction);
                transform.position = hit.point;

            }

        }

    }
}
