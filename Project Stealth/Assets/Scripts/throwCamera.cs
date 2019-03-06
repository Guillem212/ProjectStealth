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

    [Range(1000, 5000)]
    public float throwForce;

    private void Update()
    {
        if (cameraFixed)
        {
            if (Input.GetButtonDown("ThrowCamera"))
            {
                //Activa y desactiva la camara.
                cameraPlayer.SetActive(!cameraPlayer.activeSelf);
                lookingCamera = !cameraPlayer.activeSelf;
            }
            if (lookingCamera)
            {
                //saca el input del raton o del mando.
                mouseInput = Input.GetAxis("Mouse X");
                lookhere = new Vector3(0, mouseInput, 0);

                //Comprueba que está dentro de los limites de la camara, si lo esta, se mueve.
                if (thisAttachableCamera.transform.localEulerAngles.y <= colliderCamera.max && mouseInput > 0)
                    thisAttachableCamera.transform.Rotate(lookhere);
                else if(thisAttachableCamera.transform.localEulerAngles.y >= colliderCamera.min && mouseInput < 0)
                    thisAttachableCamera.transform.Rotate(lookhere);
            }
        }
        else
        {
            if (Input.GetButtonDown("ThrowCamera"))
            {
                //Spawnea y lanza la camara con una fuerza dada.
                thisAttachableCamera = Instantiate(attachableCamera, transform.position + Camera.main.transform.TransformDirection(transform.forward), Quaternion.identity);
                thisAttachableCamera.GetComponent<Rigidbody>().AddForce(Camera.main.transform.TransformDirection(transform.forward) * throwForce);
            }
        }
    }


    //Getter para poder saber si estás en la camara o no.
    public static bool getLookingCamera()
    {
        return lookingCamera;
    }
}
