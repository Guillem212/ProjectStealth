using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private GameObject player;
    Climb climbScript;

    #region lerping
    Quaternion from;
    Quaternion to;
    [HideInInspector] public bool lerping = false;
    [SerializeField] float speed = 3f;
    float turningTime = 0f;
    Transform t;
    #endregion

    private float xAxisClamp;

    float initialXPos;
    float initialYPos;    

    private void Awake()
    {
        xAxisClamp = 0.0f;
        initialXPos = transform.position.x;
        initialYPos = transform.position.y;
        climbScript = player.GetComponent<Climb>();
        t = player.GetComponent<Transform>();
    }
    
    private void Update()
    {
        //bloquea la rotación mientras se escala o se asoma
        if (!climbScript.isClimbing && !lerping) 
            CameraRotation();    
        if (Input.GetButton("CrouchRight") && !lerping && !PlayerMovement.isWalking)
        {
            lerping = true;
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, -35f));
            turningTime = 0f;
        }
        else if (Input.GetButton("CrouchLeft") && !lerping && !PlayerMovement.isWalking)
        {
            lerping = true;
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, 35f));
            turningTime = 0f;
        }

        if (lerping)
        {
            if (Input.GetButtonUp("CrouchLeft") || (Input.GetButtonUp("CrouchRight"))) //si está lerpeando y soltamos un botón
            {                
                from = t.rotation;
                to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, 0f));
                turningTime = 0f;
            }            
            turningTime += Time.fixedDeltaTime * speed;
            t.rotation = Quaternion.Lerp(from, to, turningTime);            

            if (t.rotation.eulerAngles.z <= 0.4f && t.rotation.eulerAngles.z >= -0.4f)
            {
                lerping = false;
            }
        }        
    }

    /*private void Update()
    {
        //bloquea la rotación mientras se escala o se asoma
        if (!climbScript.isClimbing && !lerping)
            CameraRotation();
        if (Input.GetButtonDown("CrouchRight"))
        {
            lerping = true;
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, -35f));
            turningTime = 0f;
        }
        else if (Input.GetButtonUp("CrouchRight"))
        {
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, 0f));
            turningTime = 0f;
        }
        else if (Input.GetButtonDown("CrouchLeft"))
        {
            lerping = true;
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, 35f));
            turningTime = 0f;
        }
        else if (Input.GetButtonUp("CrouchLeft"))
        {
            from = t.rotation;
            to = Quaternion.Euler(new Vector3(t.rotation.eulerAngles.x, t.rotation.eulerAngles.y, 0f));
            turningTime = 0f;
        }


        if (lerping)
        {
            turningTime += Time.fixedDeltaTime * speed;
            t.rotation = Quaternion.Lerp(from, to, turningTime);
            print(t.rotation == to);
            if (t.rotation.eulerAngles.z <= 0.5f)
            {
                lerping = false;
            }
        }
    }*/

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
