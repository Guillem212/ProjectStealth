using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    private float minPitch = -80f;
    private float maxPitch = 60f;

    private float xAxisClamp;

    private void Awake()
    {
        xAxisClamp = 0.0f;
    }
    
    private void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //mouse X = input name
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY; //la suma final es +90 mirando hacia arriba y -90 hacia abajo
        //print(xAxisClamp);

        if (xAxisClamp > 70.0f)
        {
            xAxisClamp = 70.0f;
            mouseY = 0.0f; //anula transform.rotate
            ClampXAxisRotationToValue(280.0f);
        }
        else if (xAxisClamp < -70.0f)
        {
            xAxisClamp = -70.0f;
            mouseY = 0.0f; //anula transform.rotate
            ClampXAxisRotationToValue(70.0f);
        }
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value) //previene que la camara no se salte el clamp para evitar bugs
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
