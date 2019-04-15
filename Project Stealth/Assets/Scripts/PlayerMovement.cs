using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    private float initialSpeed = 6.0f;    
    public float gravity = 20.0f;
    GameObject hookHolder;
    private Animator animator;    

    //jump
    private bool isJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;    

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

    Vector3 forwardMovement;
    Vector3 rightMovement;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        hookHolder = GameObject.Find("HookHolder");

        animator = GameObject.Find("Character").GetComponent<Animator>();
    }

    void Update()
    {

        if (!GrappingHook.hookedIntoAnObject && !AttachCameraBehaviour.getLookingCamera())
        {
            speed = initialSpeed;
            //cameraRotate();            
            if (controller.isGrounded)
            {
                if (Input.GetButton("Sprint"))
                {
                    speed = speed * 2;
                    if (animator.GetBool("Crouched"))
                    { StandUp(); }
                }

                else if (Input.GetButtonDown("Crouch")) 
                {
                    if(!animator.GetBool("Crouched")) //si no está agachado
                    {
                        animator.SetTrigger("ToCrouch");                        
                        animator.SetBool("Crouched", true);
                        controller.radius = 1f;
                        controller.center = new Vector3(0, -0.5f, 0);                        
                    }
                    else //si está agachado
                    {
                        StandUp();                        
                    }
                }

                if(Input.GetButtonDown("Jump") && !isJumping)
                {
                    isJumping = true;
                    StartCoroutine(JumpEvent());
                }

                if (animator.GetBool("Crouched"))
                {
                    speed = speed / 2;
                }

                float vertInput = Input.GetAxis("Vertical") * speed;
                float horizInput = Input.GetAxis("Horizontal") * speed;

                forwardMovement = transform.forward * vertInput;
                rightMovement = transform.right * horizInput;
            }            
            controller.SimpleMove(forwardMovement + rightMovement);
        }        
    }

    void StandUp()
    {
        controller.radius = 2f;
        controller.center = new Vector3(0, 0, 0);
        animator.SetBool("Crouched", false);
    }

    void cameraRotate()
    {
        yaw += Input.GetAxis("Mouse X") * SpeedH;
        pitch -= Input.GetAxis("Mouse Y") * SpeedV;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        virtualCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        playerModel.transform.localEulerAngles =new Vector3(0, virtualCamera.transform.localEulerAngles.y, 0);

    }  

    private IEnumerator JumpEvent()
    {
        float timeInAir = 0.0f;
        float jumpForce;
        do
        {
            jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above); //si toca techo cae de inmediato

        isJumping = false;
        
    }

}
