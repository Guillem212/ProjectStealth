using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwCamera : MonoBehaviour
{

    public GameObject attachableCamera;
    private GameObject thisAttachableCamera;

    public LayerMask layerMask;

    private bool cameraIn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !cameraIn)
        {
            cameraIn = true;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                thisAttachableCamera = Instantiate(attachableCamera, hit.point, Quaternion.identity);
            }
        }
    }
}
