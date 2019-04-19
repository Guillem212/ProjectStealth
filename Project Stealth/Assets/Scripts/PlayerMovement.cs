using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float initialSpeed = 5.0f;
    [SerializeField] private float gravity = 20.0f;
    GameObject hookHolder;
    ArmsAnimatorBehabior armsAnim;

    //jump
    private bool isJumping;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;    

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Climb climbScript;


    public float SpeedH = 10f;
    public float SpeedV = 10f;

    private float yaw = 0f;
    private float pitch = 0f;
    private float minPitch = -80f;
    private float maxPitch = 60f;

    public GameObject virtualCamera;
    public GameObject playerModel;

    private bool crouched;

    Vector3 forwardMovement;
    Vector3 rightMovement;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        armsAnim = GetComponent<ArmsAnimatorBehabior>();
        climbScript = GetComponent<Climb>();

        Cursor.lockState = CursorLockMode.Locked;
        hookHolder = GameObject.Find("HookHolder");
        Crouch();
    }

    void Update()
    {
        
        crouched = ArmsAnimatorBehabior.rightArmAnimator.GetBool("Crouched"); //mejor si se guarda una única vez        
        if (!GrappingHook.HookedIntoAnObject && !AttachCameraBehaviour.getLookingCamera() && !Climb.isLerping)
        {
            speed = initialSpeed;
            //cameraRotate();
            if (controller.isGrounded)
            {
                if (Input.GetButton("Sprint")) //BORRAR AL FINAL
                {
                    speed = speed * 1.5f;
                    if (crouched) { StandUp(); }
                }

                else if (Input.GetButtonDown("Crouch"))
                {
                    if (!crouched)
                    {
                        Crouch();                        
                    }
                    else //si está agachado
                    {
                        StandUp();
                    }
                }

                if (Input.GetButtonDown("Jump") && !isJumping) //Cambiar
                {                    
                    if (Climb.readyToClimb && climbScript.CheckWallNormalForClimb()) { climbScript.StartClimbCoroutine(); } //readytoclimb = el climb sensor se encuentra dentro de un borde
                    else {
                        StartCoroutine(JumpEvent());
                        isJumping = true;
                    }                    
                }

                if (crouched)
                {
                    speed = speed / 2;
                }

                float vertInput = Input.GetAxis("Vertical") * speed;
                vertInput = (vertInput > 0) ? vertInput : vertInput * 0.5f;
                
                float horizInput = Input.GetAxis("Horizontal") * speed*0.6f;                

                if (vertInput > 0 && crouched)
                {
                    armsAnim.SteathWalk(true); 
                }
                else { armsAnim.SteathWalk(false); }

                forwardMovement = transform.forward * vertInput;
                rightMovement = transform.right * horizInput;
            }
            controller.SimpleMove(forwardMovement + rightMovement);
        }
    }

    public void Crouch()
    {
        //saca las manos
        armsAnim.ShowHands(true);
        controller.height = 0.8f;
    }

    void StandUp()
    {
        //desactiva los colliders de los brazos para evitar triggers innecesarios
        armsAnim.ShowHands(false);

        controller.height = 2f;
    }

    void cameraRotate() //SE USA?
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
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above && !GrappingHook.HookedIntoAnObject); //si toca techo cae de inmediato        
        isJumping = false;
        
    }
    /*
    private IEnumerator JumpToEdge()
    {
        GrappingHook.hookedIntoAnObject = true;
        float timeInAir = 0.0f;
        float jumpForce;
        do
        {
            jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (timeInAir < 0.2f && !controller.isGrounded);        
        isJumping = false;
        climbScript.StartClimbCoroutine();
        GrappingHook.hookedIntoAnObject = false;
    }*/

}
