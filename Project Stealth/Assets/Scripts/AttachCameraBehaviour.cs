using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCameraBehaviour : MonoBehaviour
{

    public GameObject attachableCamera;
    private GameObject thisAttachableCamera;

    public GameObject cameraPlayer;

    public LayerMask cameraMask;

    [HideInInspector]
    public static bool cameraFixed = false;

    private static bool lookingCamera = false;

    private float mouseInput;
    private Vector3 lookhere;

    [Range(1000, 5000)]
    public float throwForce;

    private float timeAlive = 4f;
    public static bool cameraThrowed = false;

    private Vector2 cameraRotationLimits;
    private Vector3 offset;

    private void Update()
    {
        if (cameraFixed)
        {
            cameraThrowed = false;
            if (Input.GetButtonDown("ActiveCamera"))
            {
                //Activa y desactiva la camara.
                cameraPlayer.SetActive(!cameraPlayer.activeSelf);
                lookingCamera = !cameraPlayer.activeSelf;
            }
            if (Input.GetButtonDown("ThrowCamera"))
            {
                destroyCamera();
            }
        }
        else
        {
            if (Input.GetButtonDown("ThrowCamera") && !cameraThrowed)
            {
                //Spawnea y lanza la camara con una fuerza dada.
                offset = new Vector3(transform.position.x, transform.position.y + 0.5f , transform.position.z);
                thisAttachableCamera = Instantiate(attachableCamera, offset + Camera.main.transform.TransformDirection(transform.forward), Quaternion.identity);
                thisAttachableCamera.GetComponent<Rigidbody>().AddForce(Camera.main.transform.TransformDirection(transform.forward) * throwForce);
                cameraThrowed = true;
            }

            if (cameraThrowed)
            {
                timeAlive -= Time.deltaTime;
                if (timeAlive <= 0 && !cameraFixed)
                {
                    Destroy(thisAttachableCamera);
                    cameraThrowed = false;
                    timeAlive = 4f;
                }
            }
        }

    }

    void destroyCamera()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 2f, cameraMask))
        {
            Destroy(thisAttachableCamera);
            cameraFixed = false;
            lookingCamera = false;
        }
    }

    //Getter para poder saber si estás en la camara o no.
    public static bool getLookingCamera()
    {
        return lookingCamera;
    }
}
