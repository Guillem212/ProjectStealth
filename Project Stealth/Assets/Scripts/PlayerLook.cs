using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private GameObject player;
    private float minPitch = -80f;
    private float maxPitch = 60f;
    Climb climbScript;

    //23.658
    private float xAxisClamp;

    float initialXPos;
    float initialYPos;

    private void Awake()
    {
        xAxisClamp = 0.0f;
        initialXPos = transform.position.x;
        initialYPos = transform.position.y;
        climbScript = player.GetComponent<Climb>();
    }
    
    private void Update()
    {
        //bloquea la rotación mientras se escala
        if (!climbScript.isClimbing)
            CameraRotation();    
        /*if (Input.GetButtonDown("CrouchLeft"))
        {
            player.transform.Rotate(0, 0, 25.0f );
        }
        if (Input.GetButtonDown("CrouchRight"))
        {
            player.transform.Rotate(0, 0, -25.0f);
        }
        else {*/
        //transform.position = new Vector3(initialXPos, initialYPos, transform.position.z);
        //player.transform.Rotate(0, 0, -25.0f);            
        //}
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //mouse X = input name
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY; //la suma final es +70 mirando hacia arriba y -70 hacia abajo
        //print(xAxisClamp);

        if (xAxisClamp > 70.0f)
        {
            xAxisClamp = 70.0f;
            mouseY = 0.0f; //anula transform.rotate
            ClampXAxisRotationToValue(290.0f);
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

    public void FixClamp(float rotationValue)
    {
        xAxisClamp = -rotationValue;
    }
}
