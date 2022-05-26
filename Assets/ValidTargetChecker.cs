using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidTargetChecker : MonoBehaviour
{
    public Material unknownMaterial;
    public Material unoccupiedMaterial;
    public Material occupiedMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //int layerMask = 1 << 16;

        Vector3 down = transform.TransformDirection(Vector3.down);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit, 10))
        {
            Texture2D getTx = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;
            var pixelUV = hit.textureCoord;
            pixelUV.x *= getTx.width;
            pixelUV.y *= getTx.height;
            Color c = getTx.GetPixel((int)pixelUV.x, (int)pixelUV.y);

            Debug.Log(c);
        }
        else
        {
            Debug.Log("No Hit");
        }


    }
}
