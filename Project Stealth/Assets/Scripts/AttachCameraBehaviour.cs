using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCameraBehaviour : MonoBehaviour
{

    public GameObject attachableCamera;
    private GameObject thisAttachableCamera;

    public GameObject cameraPlayer;

    public LayerMask cameraMask;

    public static bool cameraFixed = false;
    private static bool lookingCamera = false;

    private float mouseInput;
    private Vector3 lookhere;

    [Range(1000, 5000)]
    public float throwForce;

    private float timeAlive = 4f;
    public static bool cameraThrowed = false;

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
                RaycastHit hit;
                Debug.DrawRay(transform.position, Camera.main.transform.TransformDirection(Vector3.forward * 5), Color.green);
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, 5f, cameraMask))
                {
                    Destroy(thisAttachableCamera);
                    cameraFixed = false;
                    lookingCamera = false;
                }
            }

            if (lookingCamera)
            {
                //saca el input del raton o del mando.
                mouseInput = Input.GetAxis("Mouse X");
                lookhere = new Vector3(0, mouseInput, 0);

                //Comprueba que está dentro de los limites de la camara, si lo esta, se mueve.
                if (thisAttachableCamera.transform.localEulerAngles.y <= CollisionCamera.max && mouseInput > 0)
                    thisAttachableCamera.transform.Rotate(lookhere);
                else if(thisAttachableCamera.transform.localEulerAngles.y >= CollisionCamera.min && mouseInput < 0)
                    thisAttachableCamera.transform.Rotate(lookhere);
            }
        }
        else
        {
            if (Input.GetButtonDown("ThrowCamera") && !cameraThrowed)
            {
                //Spawnea y lanza la camara con una fuerza dada.
                thisAttachableCamera = Instantiate(attachableCamera, transform.position + Camera.main.transform.TransformDirection(transform.forward), Quaternion.identity);
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


    //Getter para poder saber si estás en la camara o no.
    public static bool getLookingCamera()
    {
        return lookingCamera;
    }
}
