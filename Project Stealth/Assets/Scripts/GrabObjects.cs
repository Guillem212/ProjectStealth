using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{

    [SerializeField] private float throwForce;
    [SerializeField] private float minDistance;
    private GameObject objectToGrab;
    [SerializeField] private LayerMask layer;

    [HideInInspector] public bool grabbingAnObject = false;
    private bool touched;

    float GetThrowForce() { return throwForce; }

    public float throwForce_P
    {
        get { return throwForce; }
        set { throwForce = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("GrabObject") && !grabbingAnObject)
        {            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, minDistance, layer))
            {
                print("true");
                grabbingAnObject = true;
                objectToGrab = hit.transform.gameObject;                
                StartCoroutine("IsTrigger");
                objectToGrab.GetComponent<Rigidbody>().isKinematic = true;                                                                
                objectToGrab.transform.parent = Camera.main.transform;                
            }            
        }

        if (grabbingAnObject)
        {
            //print("cargando objeto");
            if (Input.GetButtonDown("ThrowObject")) //click alt -> lanzar
            {
                objectToGrab.GetComponent<GrabbableObject>().Throw();
            }
            else if (Input.GetButtonUp("GrabObject"))
            {
                print("soltar");
                objectToGrab.GetComponent<GrabbableObject>().ToTheGround();
            }
        }        
    }

    IEnumerator IsTrigger()
    {
        yield return new WaitForSeconds(0.3f);
        objectToGrab.GetComponent<BoxCollider>().isTrigger = true;
        print("hola");
    }
}
