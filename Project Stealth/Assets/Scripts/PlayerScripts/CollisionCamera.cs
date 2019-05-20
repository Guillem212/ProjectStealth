using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CollisionCamera : MonoBehaviour
{
    private Rigidbody rb_Camera;

    //Punto en el que impacta la camara.
    private ContactPoint point;

    private void Start()
    {
        rb_Camera = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb_Camera.constraints = RigidbodyConstraints.FreezePosition;
            rb_Camera.useGravity = false;            

            //Hace que no puedas lanzar mas de una camara.
            AttachCameraBehaviour.cameraFixed = true;

            //Saca la normal de el punto de colision y establece la rotación inicial
            transform.rotation = Quaternion.LookRotation(collision.GetContact(collision.contactCount - 1).normal, Vector3.up);                        
        }
        else
        { 
            // Mas adelante implementar animación de destruir camara.
            Destroy(gameObject);
            AttachCameraBehaviour.cameraThrowed = false;
        }        
    }
}
