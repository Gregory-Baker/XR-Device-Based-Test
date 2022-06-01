using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidTargetChecker : MonoBehaviour
{
    [SerializeField]
    Material unknownMaterial;
    [SerializeField]
    Material unoccupiedMaterial;
    [SerializeField]
    Material occupiedMaterial;

    Material myMaterial;

    Vector3 offset = new Vector3(0, 0.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Material>();   
    }

    // Update is called once per frame
    void Update()
    {

        int layerMask = 1 << 16;

        Vector3 down = Vector3.down;

        

        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, down, out hit, 10, layerMask))
        {
            Texture2D getTx = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;
            var pixelUV = hit.textureCoord;
            pixelUV.x *= getTx.width;
            pixelUV.y *= getTx.height;
            Color c = getTx.GetPixel((int)pixelUV.x, (int)pixelUV.y);

            Debug.Log(c);
            if (c.r > 0.1)
            {
                myMaterial = occupiedMaterial;
            } else
            {
               myMaterial = unoccupiedMaterial;
            }
        }
        else
        {
            //Debug.Log("No Hit");
        }


    }
}
