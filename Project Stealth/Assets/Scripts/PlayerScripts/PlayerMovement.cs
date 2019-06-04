using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject hookHolder;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject postProcessing;
    public GameObject virtualCamera;
    public GameObject playerModel;
    [Space]
    [Header("Parameters")]
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;    
    public static bool isWalking = false;

    private float initialSpeed = 5.0f;
    ArmsAnimatorBehabior armsAnim;

    public static bool allowedToMove = true;

    //states
    private bool isJumping;
    public static bool crouched;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Climb climbScript;
    private PlayerLook pl;

    Vector3 forwardMovement;
    Vector3 rightMovement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        armsAnim = GetComponent<ArmsAnimatorBehabior>();
        climbScript = GetComponent<Climb>();
        pl = playerCam.GetComponent<PlayerLook>(); //BORRAR?

        Cursor.lockState = CursorLockMode.Locked;
        Crouch();
    }

    void Update()
    {        
        crouched = ArmsAnimatorBehabior.rightArmAnimator.GetBool("Crouched"); //mejor si se guarda una única vez
        if (!GrappingHook.HookedIntoAnObject && !AttachCameraBehaviour.getLookingCamera() && !Climb.isLerping && !pl.lerping && allowedToMove)
        {            
            speed = initialSpeed;
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

                if (Input.GetButtonDown("Jump") && !isJumping && !climbScript.isClimbing) //Cambiar
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
            isWalking = (forwardMovement != Vector3.zero || rightMovement != Vector3.zero); //para asomarse por las esquinas
        }
    }

    public void Crouch()
    {
        //saca las manos
        armsAnim.ShowHands(true);
        controller.height = 0.8f;
        postProcessing.GetComponent<PPEffects>().SetVignetteLayer(true);
    }

    void StandUp()
    {
        //desactiva los colliders de los brazos para evitar triggers innecesarios
        armsAnim.ShowHands(false);
        postProcessing.GetComponent<PPEffects>().SetVignetteLayer(false);
        controller.height = 2f;
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
}
