using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CollisionCamera : MonoBehaviour
{
    private Rigidbody rb_Camera;

    //Punto en el que impacta la camara.
    private ContactPoint point;

    private float initialCameraRotation;
    public static float min, max;

    private void Start()
    {
        rb_Camera = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rb_Camera.constraints = RigidbodyConstraints.FreezePosition;
            rb_Camera.useGravity = false;

            //Hace que no puedas lanzar mas de una camara.
            AttachCameraBehaviour.cameraFixed = true;

            //Saca la normal de el punto de colision.
            calculeAngle(collision.GetContact(collision.contactCount - 1).normal);
        }
        else
        { // Mas adelante implementar animación de destruir camara.
            Destroy(gameObject);
            AttachCameraBehaviour.cameraThrowed = false;
        }
    }

    void calculeAngle(Vector3 contactPoint)
    {
        //calcula el angulo inicial de la camara.
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);

        initialCameraRotation = transform.localEulerAngles.y;

        print("Minimo: " + min + " Maximo: " + max + " Actual: " + initialCameraRotation);
        //Calcula los limites de la camara.
        min = initialCameraRotation - 80f; if (min < 0) min += 360;
        max = initialCameraRotation + 80f; if (max > 360) max -= 360;


    }
}
