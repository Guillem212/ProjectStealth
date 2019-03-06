using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwCamera : MonoBehaviour
{

    public GameObject attachableCamera;
    private GameObject thisAttachableCamera;

    public GameObject cameraPlayer;

    public LayerMask layerMask;

    public static bool cameraFixed = false;
    private static bool lookingCamera = false;

    private float mouseInput;
    private Vector3 lookhere;

    private void Update()
    {
        if (cameraFixed)
        {
            if (Input.GetButtonDown("ThrowCamera"))
            {
                cameraPlayer.SetActive(!cameraPlayer.activeSelf);
                lookingCamera = !cameraPlayer.activeSelf;
            }
            if (lookingCamera)
            {
                mouseInput = Input.GetAxis("Mouse X");
                lookhere = new Vector3(0, mouseInput, 0);
                if (thisAttachableCamera.transform.rotation.y > colliderCamera.min && mouseInput < 0)
                {
                    thisAttachableCamera.transform.Rotate(lookhere);
                }
                else if(thisAttachableCamera.transform.rotation.y < colliderCamera.max && mouseInput > 0)
                {
                    thisAttachableCamera.transform.Rotate(lookhere);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("ThrowCamera"))
            {
                thisAttachableCamera = Instantiate(attachableCamera, transform.position + Camera.main.transform.TransformDirection(transform.forward), Quaternion.identity);
                thisAttachableCamera.GetComponent<Rigidbody>().AddForce(Camera.main.transform.TransformDirection(transform.forward) * 1000);
            }
        }
    }


    //Getter para poder saber si estás en la camara o no.
    public static bool getLookingCamera()
    {
        return lookingCamera;
    }
}
