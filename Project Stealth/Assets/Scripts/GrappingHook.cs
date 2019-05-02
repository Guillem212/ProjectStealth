using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour
{
    #region variables
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private GameObject hookHolder;        
    private GameObject hook;
    CharacterController characterController;
    private Vector3 hookDirection;
    public float hookOffset; 
    [SerializeField] private float hookTravelSpeed;
    [SerializeField] private float playerTravelSpeed;
    Climb climbScript;

    public static bool PlayerHasFiredTheHook; //se accede desde demasiados sitios
    public static bool HookedIntoAnObject; 

    private float timer = 0f;
    private float timerLimit = 0.4f;
    #endregion

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        climbScript = GetComponent<Climb>();
    }

    void Update()
    {                        
        if (Input.GetButtonDown("ThrowHook") && !PlayerHasFiredTheHook && climbScript.CheckWallNormalForHook()) //checkWallNormal(bool onlyJump) ,en este caso queremos usar el gancho
        {                       
            //llegado a este punto ya tenemos la normal del muro que vamos a trepar
            PlayerHasFiredTheHook = true;
            hook = Instantiate(hookPrefab, hookHolder.transform.position, Camera.main.transform.rotation); //se instancia el gancho en la dirección de la cámara            
        }

        if (PlayerHasFiredTheHook && !HookedIntoAnObject) //mientars el gancho está en el aire
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed); //desplazamiento            
            if (Vector3.Distance(transform.position, hook.transform.position) > climbScript.maxClimbingDistance + 1f) DestroyHook(); //temporal para evitar bugs, funciona igual sin el
        }

        if (HookedIntoAnObject && PlayerHasFiredTheHook) //si se ha enganchado (lo decide HookDetector.cs)
        {
            {
                if (timer >= timerLimit)
                {
                    timer = timerLimit;
                    transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed); //desplaza al jugador            
                    float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

                    if (distanceToHook < hookOffset)
                    {
                        PlayerHasFiredTheHook = false;
                        climbScript.StartClimbCoroutine();
                        timer = 0f; //restart timer
                    }                    
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            
        }        
    }   

    public void DestroyHook()
    {
        Destroy(hook);
        PlayerHasFiredTheHook = false;
        HookedIntoAnObject = false;
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
