using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardTeleopHandler : MonoBehaviour
{
    public Camera raycastCamera;

    public ZEDManager zed;
    sl.ZEDCamera zedCamera;


    // Start is called before the first frame update
    void Start()
    {
        zedCamera = zed.zedCamera;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = worldPosition;

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.x *= (float)zedCamera.ImageWidth / (float)Screen.width;
            mousePosition.y *= (float)zedCamera.ImageHeight / (float)Screen.height;

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
