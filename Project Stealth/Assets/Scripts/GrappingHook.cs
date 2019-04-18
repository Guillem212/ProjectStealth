using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private GameObject hookHolder;
    [SerializeField] private GameObject hookedObject;
    [SerializeField] private LayerMask layer;
    private GameObject hook;
    CharacterController characterController;
    private Vector3 hookDirection;

    [SerializeField] private float hookTravelSpeed;
    [SerializeField] private float playerTravelSpeed;
    [SerializeField] private float maxDistance;

    Vector3 wallNormal;

    [HideInInspector]public static bool playerHasFiredTheHook; //si hemos o no disparado el gancho
    [HideInInspector]public static bool hookedIntoAnObject; //si se ha enganchado    

    private float currentDistance;

    private bool grounded;

    private bool climbingUp = false;
    private bool movingForward = false;

    [SerializeField] private float upForce; //20
    [SerializeField] private float forwardForce; //15
    #endregion

    /// EL BUEN LERPEO CUMBIÓN //////////////
    float lerpTime = 3f; //from begginning to el final 
    bool lerp = false;
    Vector3 from;
    Vector3 to;
    public static bool test = false;
    float timeStartedLerping;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            timeStartedLerping = Time.time;

            test = true;
        }
        if (Input.GetButtonDown("ThrowHook") && !playerHasFiredTheHook && CheckWallNormal()) //h de momento
        {                       
            //llegado a este punto ya tenemos la normal del muro que vamos a trepar
            playerHasFiredTheHook = true;
            hook = Instantiate(hookPrefab, hookHolder.transform.position, Camera.main.transform.rotation); //se instancia el gancho en la dirección de la cámara            
        }

        if (playerHasFiredTheHook && !hookedIntoAnObject) //mientars el gancho está en el aire
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed); //desplazamiento            
            if (Vector3.Distance(transform.position, hook.transform.position) > maxDistance + 1f) DestroyHook(); //temporal para evitar bugs, funciona igual sin el
        }

        if (hookedIntoAnObject && playerHasFiredTheHook) //si se ha enganchado (lo decide HookDetector.cs)
        {            
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed); //desplaza al jugador            
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            if (distanceToHook < 2)
            {                
                StartCoroutine(Climb());

                if (lerp)
                {/*
                    currentLerpTime += Time.deltaTime; //incremento del tiempo
                    if (currentLerpTime >= lerpTime)
                    { currentLerpTime = lerpTime; }

                    float perc = currentLerpTime / lerpTime;
                    transform.position = Vector3.Lerp(from, to, perc);*/
                }                               
            }
        }
        if (test)
        {            
            StartCoroutine(TEST());

            transform.position = Lerp(from, to, timeStartedLerping, lerpTime);
        }
    }

    //Vector3 temp = new Vector3(0, hookedObject.transform.position.y, 0);
    //transform.position += temp;

    IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.5f);
        lerp = true;
        from = transform.position;
        to = Vector3.up * upForce;

        //playerHasFiredTheHook = false; posicion inicial
        transform.rotation = Quaternion.LookRotation(-wallNormal, Vector3.up);        
        
        transform.position = new Vector3(transform.position.x, hookedObject.transform.position.y, transform.position.z);

        yield return new WaitForSeconds(2f); //mientras sube, luego se desactiva

        playerHasFiredTheHook = false;
        test = false;

        //transform.Translate(Vector3.up * upForce);
        //transform.Translate(-(wallNormal) * forwardForce, Space.World);
        DestroyHook();
    }

    IEnumerator TEST()
    {
        lerp = true; //2 segundos para lerp
        from = transform.position;
        to = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        yield return new WaitForSeconds(2f); //mientras sube, luego se desactiva

        test = false;
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;

        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        return result;
    }

    public void SetHookedObject(GameObject hookableObj)
    {
        hookedObject = hookableObj;
    }    

    void DestroyHook()
    {
        Destroy(hook);
        playerHasFiredTheHook = false;
        hookedIntoAnObject = false;
    }

    private bool CheckWallNormal()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, maxDistance, layer)) //si no hemos clicado un objeto enganchable no devuelve vector y no instancia ningun gancho
        {
            //rayo de la normal
            Debug.DrawRay(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), Color.red, 3f);
            Debug.DrawRay(hit.point, hit.normal, Color.green, 3f);
            wallNormal = hit.normal;
            return true;
        }
        return false; 
    }
}

#region Mis Putas MIerdas
/*
 
    void CheckIfGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            grounded = true;
        }
        else { grounded = false; }
        print(grounded);
    }

    int checkWall() //1 si es pared normal, 0 si no es nada, 7 si es hookable
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.transform.tag == "Hookable") return 7;
            else return 1;
        }
        return 0;
    }

    private void CheckWallNormal()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) )
        {
            //vector desde la camara hasta el punto en el que se ha clicado
            Vector3 incomingVec = hit.point - hookHolder.transform.position;        
            //vector de reflexion a partir de la normal del punto  
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

            // linea de disparo
            Debug.DrawLine(hookHolder.transform.position, hit.point, Color.red);
            //linea de reflejo
            Debug.DrawRay(hit.point,reflectVec, Color.green, 1f);
            //rayo de la normal
            Debug.DrawRay(hit.point, hit.normal, Color.green, 3f);        
        }
    }

    IEnumerator 3SecondsCoroutine()
    {
        
        float duration = Time.time + 3.0f;
        while (Time.time < duration)
        {
            print("hola");
            yield return null;
        }
        print("adios");
        yield return null;        
    }

    transform.position = Vector3.MoveTowards(transform.position, Vector3.up, Time.deltaTime * 20f); //desplaza al jugador                 
    transform.position = Vector3.MoveTowards(transform.position, -Vector3.forward, Time.deltaTime * playerTravelSpeed); //desplaza al jugador         
*/
#endregion
