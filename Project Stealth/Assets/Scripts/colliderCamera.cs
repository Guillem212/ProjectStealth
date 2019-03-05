using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class colliderCamera : MonoBehaviour
{
    public LayerMask layer;

    private Rigidbody rb_Camera;

    private void Start()
    {
        rb_Camera = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rb_Camera.constraints = RigidbodyConstraints.FreezePosition;
            rb_Camera.isKinematic = true;
            rb_Camera.useGravity = false;

            throwCamera.cameraFixed = true;
        }
        else // Mas adelante implementar animación de destruir camara
            Destroy(gameObject);
    }
}
