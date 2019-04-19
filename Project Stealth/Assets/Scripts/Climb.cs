using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject climbSpotPrefab;
    [SerializeField] private GameObject climbHolder;
    private GameObject climbSensor;
    private GameObject objectToClimb;
    [SerializeField] private float maxClimbDistance;
    [SerializeField] private float i_maxHookingDistance;    
    public float maxClimbingDistance
    {
        get { return i_maxHookingDistance; }
        set { i_maxHookingDistance = value; }
    }

    public GameObject playerCam;

    public static bool readyToClimb = false;

    [SerializeField] private LayerMask layer;

    private GameObject climbSpot;
    private GrappingHook hookScript;
    private ArmsAnimatorBehabior animator;    
    public static bool isLerping;
    public bool isClimbing { get; private set; }

    Vector3 wallNormal;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        hookScript = GetComponent<GrappingHook>();
        animator = GetComponent<ArmsAnimatorBehabior>();
        climbSensor = GameObject.Find("ClimbSensor");
    }

    void LateUpdate()
    {
        if (isLerping)
        {            
            transform.position = Vector3.Slerp(transform.position, climbSpot.transform.position, 0.05f);
            if (Vector3.Distance(transform.position, climbSpot.transform.position) < 0.5f) {
                isLerping = false;
                isClimbing = false;
                Destroy(climbSpot);                
                playerCam.GetComponent<PlayerLook>().FixClamp(playerCam.transform.eulerAngles.x - 360f); //angulo de llegada para ajustar el clamp
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
        //enganchar brazos
        ArmsAnimatorBehabior.leftArmAnimator.SetTrigger("Climb");
        ArmsAnimatorBehabior.rightArmAnimator.SetTrigger("Climb");
        playerCam.transform.LookAt(climbHolder.transform);
        yield return new WaitForSeconds(0.25f);
        isLerping = true; 
        //trepar
        if (GrappingHook.HookedIntoAnObject) //en el caso de que trepemos gracias al gancho
        {
            yield return new WaitForSeconds(0.5f);
            hookScript.DestroyHook();            
        }
    }

    public bool CheckWallNormalForHook()
    {
        RaycastHit hit;        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, i_maxHookingDistance, layer)) //si no hemos clicado un objeto enganchable no devuelve vector y no instancia ningun gancho
        {
            //rayo de la normal
            wallNormal = hit.normal;
            objectToClimb = hit.collider.gameObject; //si lanzamos el gancho el hookedObject lo coje el raycast, si escalamos lo coge el climb detector
            return true;
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
}
