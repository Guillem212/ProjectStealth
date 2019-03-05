using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwCamera : MonoBehaviour
{

    public GameObject attachableCamera;
    private GameObject thisAttachableCamera;

    public GameObject camera;

    public LayerMask layerMask;

    public static bool cameraFixed = false;

    private void Update()
    {
        if (Input.GetButtonDown("ThrowCamera") && !cameraFixed)
        {
            thisAttachableCamera = Instantiate(attachableCamera, transform.position + Camera.main.transform.TransformDirection(transform.forward), Quaternion.identity);
            thisAttachableCamera.GetComponent<Rigidbody>().AddForce(Camera.main.transform.TransformDirection(transform.forward) * 1000);
        }

        if (Input.GetButtonDown("ThrowCamera") && cameraFixed)
        {
            camera.SetActive(!camera.activeSelf);
        }

        //Falta hacer que el ppersonaje tenga que ir a recoger la camara para volverla a usar.
    }
}
