using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour {

    public GameObject hookPref;
    public GameObject hookHolder;
    public GameObject hookedObject;
    GameObject hook;
    CharacterController characterController;
    private Vector3 hookDirection;

    public float hookTravelSpeed;
    public float playerTravelSpeed;

    public static bool fired; //si hemos o no disparado el gancho
    public static bool hooked; //si se ha enganchado 

    public float maxDistance;
    private float currentDistance;

    private bool grounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update () {
        if (Input.GetMouseButtonDown(0) && !fired)
        {
            fired = true;
            hook = Instantiate(hookPref, hookHolder.transform.position,Camera.main.transform.rotation); //se instancia el gancho en la dirección de la cámara
        }

        if (fired && !hooked) //mientars el gancho está en el aire
        {            
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed); //desplazamiento
            currentDistance = Vector3.Distance(transform.position, hook.transform.position); //comprueba distancia con el jugador para destruirse si se aleja demasiado

            if (currentDistance >= maxDistance)
            { ReturnHook(); } //destruir si se aleja
        }

        if (hooked && fired ) //si se ha enganchado (lo decide HookDetector.cs)
        {                                       
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed); //desplaza al jugador            
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            //this.GetComponent<Rigidbody>().useGravity = false;

            if (distanceToHook < 2)
            {
                if (!grounded)
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 13f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 18f);
                }

                StartCoroutine("Climb");
            }
        }
        else {
            //this.GetComponent<Rigidbody>().useGravity = true;
            //hook.transform.parent = hookHolder.transform;
        }
        
	}

    IEnumerator Climb()
    {        
        yield return new WaitForSeconds(0.1f);
        ReturnHook();
    }

    void ReturnHook()
    {        
        Destroy(hook);
        //hook.transform.rotation = hookHolder.transform.rotation;
        //hook.transform.position = hookHolder.transform.position; //se puede cambiar respecto a la camara del jugador

        fired = false;
        hooked = false;
    }

    void CheckiFGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            grounded = true;
        }
        else { grounded = false; }
    }
}
