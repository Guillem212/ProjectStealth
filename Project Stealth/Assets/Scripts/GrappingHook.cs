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
    #endregion

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) { Debug.Log(CheckWallNormal()); }
        
        if (Input.GetButtonDown("ThrowHook") && !playerHasFiredTheHook && CheckWallNormal()) //h de momento
        {                       
            //llegado a este punto ya tenemos la normal del muro que vamos a trepar
            playerHasFiredTheHook = true;
            hook = Instantiate(hookPrefab, hookHolder.transform.position, Camera.main.transform.rotation); //se instancia el gancho en la dirección de la cámara            
        }

        if (playerHasFiredTheHook && !hookedIntoAnObject) //mientars el gancho está en el aire
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed); //desplazamiento            
        }

        if (hookedIntoAnObject && playerHasFiredTheHook) //si se ha enganchado (lo decide HookDetector.cs)
        {            
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed); //desplaza al jugador            
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            if (distanceToHook < 2)
            {
                if (!grounded) //ARREGLAR
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 13f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 18f);
                }
                StartCoroutine("Climb");
            }
        }
    }

    public void SetHookedObject(GameObject hookableObj)
    {
        hookedObject = hookableObj;
    }    

    IEnumerator Climb()
    {
        /*
        transform.position = Vector3.MoveTowards(transform.position, Vector3.up, Time.deltaTime * 20f); //desplaza al jugador         
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector3.MoveTowards(transform.position, -Vector3.forward, Time.deltaTime * playerTravelSpeed); //desplaza al jugador         
        */
        yield return new WaitForSeconds(0.1f);
        DestroyHook();
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

        if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, maxDistance, layer)) //si no hemos clicado un objeto enganchable no devuelve vector y no instancia ningun gancho
        {            
            //rayo de la normal
            Debug.DrawRay(hit.point, hit.normal, Color.green, 3f);
            wallNormal = hit.normal;
            return true;
        }
        return false; 
    }    
}

#region Funciones auxiliares
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
*/
#endregion
