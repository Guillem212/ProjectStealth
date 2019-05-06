using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    #region variables    
    [Header("GameObjects")]
    public GameObject playerCam;
    [SerializeField] private GameObject climbSpotPrefab;
    [SerializeField] private GameObject climbHolder;
    [SerializeField] private GameObject climbLooker;
    [SerializeField] private GameObject hookHolder;
    private GameObject objectToClimb;
    [Space]
    [Header("Parameters")]
    [SerializeField] private float maxClimbDistance;
    [SerializeField] private float i_maxHookingDistance;
    [SerializeField] private float minHookingDistance;
    public static bool readyToClimb = false;
    [SerializeField] private LayerMask layer;
    public float maxClimbingDistance
    {
        get { return i_maxHookingDistance; }
        set { i_maxHookingDistance = value; }
    }        
    private GameObject climbSpot;
    private GrappingHook hookScript;
    private ArmsAnimatorBehabior animator;
    [Space]
    [Header("Control")]
    public static bool isLerping;
    public bool isClimbing { get; private set; }

    Vector3 wallNormal;    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hookScript = GetComponent<GrappingHook>();
        animator = GetComponent<ArmsAnimatorBehabior>();
    }

    void LateUpdate()
    {        
        if (isLerping)
        {            
            transform.position = Vector3.Slerp(transform.position, climbSpot.transform.position, 0.3f);
            if (Vector3.Distance(transform.position, climbSpot.transform.position) < 0.2f) {
                isLerping = false;
                isClimbing = false;
                Destroy(climbSpot);
                playerCam.GetComponent<PlayerLook>().FixClamp(playerCam.transform.eulerAngles.x - 360f); //angulo de llegada para ajustar el clamp
                ArmsAnimatorBehabior.ArmsToWall(false);
                if (ArmsAnimatorBehabior.rightArmAnimator.GetBool("Crouched")) { animator.SetActiveColliders(true); } //si al llegar estamos agachados reactivar colliders de manos
            }
        }
    }    

    public void setObjectToClimb(GameObject target) //identifica el objeto a escalar (para le trigger del climbSensor)
    {
        objectToClimb = target;
    }

    public void StartClimbCoroutine()
    {
        StartCoroutine(ClimbAction());
    }

    IEnumerator ClimbAction()
    {        
        isClimbing = true;
        animator.SetActiveColliders(false); //desactiva los colliders de las manos para evitar bug de animacion
        //centra la dirección del personaje en dirección contraria al muro        
        transform.rotation = Quaternion.LookRotation(-wallNormal, Vector3.up);
        //se centra al personaje en una altura universal (hookedObject siempre tiene el mismo ancho)
        transform.position = new Vector3(transform.position.x, objectToClimb.transform.position.y, transform.position.z);
        //se instancia el punto de escalada en su respectivo holder
        climbSpot = Instantiate(climbSpotPrefab, climbHolder.transform.position, Camera.main.transform.rotation);

        playerCam.transform.LookAt(climbLooker.transform);
        //enganchar brazos
        ArmsAnimatorBehabior.leftArmAnimator.SetTrigger("Climb");
        ArmsAnimatorBehabior.rightArmAnimator.SetTrigger("Climb");
        yield return null;
    }

    public bool CheckWallNormalForHook()
    {
        RaycastHit hit;
        //si no hemos clicado un objeto enganchable no devuelve vector y no instancia ningun gancho
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, i_maxHookingDistance, layer) && 
            (hit.normal != Vector3.up && hit.normal != -Vector3.up)) 
        {                        
            //rayo de la normal
            wallNormal = hit.normal;
            Debug.DrawRay(hit.point, Vector3.up, Color.red, 2f);
            objectToClimb = hit.collider.gameObject; //si lanzamos el gancho el hookedObject lo coje el raycast, si escalamos lo coge el climb detector            
            GrappingHook.distanceForHook = Vector3.Distance(hookHolder.transform.position, hit.transform.position);
            return GrappingHook.distanceForHook > minHookingDistance;
        }
        return false;

    }

    public bool CheckWallNormalForClimb()
    {
        RaycastHit hit;        
        if (Physics.Raycast(Camera.main.transform.position, transform.forward, out hit, maxClimbDistance))
        {
            //rayo de la normal
            wallNormal = hit.normal;            
            return true;
        }
        return false;
    }

    public void EventOccured() //animacion de escalada
    {
        isLerping = true;
        //trepar
        if (GrappingHook.HookedIntoAnObject) //en el caso de que trepemos gracias al gancho
        {            
            hookScript.DestroyHook();
        }        
    }
}
