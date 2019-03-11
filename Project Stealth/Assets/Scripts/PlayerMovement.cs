using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    GameObject hookHolder;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;


    public float SpeedH = 10f;
    public float SpeedV = 10f;

    private float yaw = 0f;
    private float pitch = 0f;
    private float minPitch = -80f;
    private float maxPitch = 60f;

    public GameObject virtualCamera;
    public GameObject playerModel;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        hookHolder = GameObject.Find("HookHolder");
    }

    void Update()
    {        
        if (!GrappingHook.hookedIntoAnObject && !AttachCameraBehaviour.getLookingCamera())
        {
            cameraRotate();
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection = Camera.main.transform.TransformDirection(moveDirection);
                moveDirection = moveDirection * speed;
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

            controller.Move(moveDirection * Time.deltaTime);
        }        
    }

    void cameraRotate()
    {
        yaw += Input.GetAxis("Mouse X") * SpeedH;
        pitch -= Input.GetAxis("Mouse Y") * SpeedV;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        virtualCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        playerModel.transform.localEulerAngles =new Vector3(0, virtualCamera.transform.localEulerAngles.y, 0);

    }
}
