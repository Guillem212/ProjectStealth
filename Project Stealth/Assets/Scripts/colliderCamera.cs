using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class colliderCamera : MonoBehaviour
{
    public LayerMask layer;

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
            throwCamera.cameraFixed = true;

            //Saca la normal de el punto de colision.
            calculeAngle(collision.GetContact(collision.contactCount - 1).normal);
        }
        else // Mas adelante implementar animación de destruir camara.
            Destroy(gameObject);
    }

    void calculeAngle(Vector3 contactPoint)
    {
        //calcula el angulo inicial de la camara.
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint) * transform.rotation;

        initialCameraRotation = transform.localEulerAngles.y;

        //Calcula los limites de la camara.
        min = initialCameraRotation - 80f; if (min < 0) min += 360;
        max = initialCameraRotation + 80f; if (max > 360) max -= 360;

        //Si la camara va a salir del reves, la pone del derecho.
        if (contactPoint.z < 0)
            transform.Rotate(0, 0, 180);


    }
}
