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

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        hookHolder = GameObject.Find("HookHolder");
    }

    void Update()
    {        
        if (!GrappingHook.hooked && !AttachCameraBehaviour.getLookingCamera())
        {
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
}
