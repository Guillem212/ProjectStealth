using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{

    [SerializeField] private float throwForce;
    [SerializeField] private float minDistance; //distancia minima hasta el objeto
    private GameObject objectToGrab;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject rightArm;    
    float throwTemporizer;
    bool throwRequested;    

    Vector3 armLocalPosition;

    [HideInInspector] public bool grabbingAnObject = false;
    private bool touched;

    float GetThrowForce() { return throwForce; }

    public float throwForce_P
    {
        get { return throwForce; }
        set { throwForce = value; }
    }

    void Update()
    {
        /*if (grabbingAnObject)
        {
            armLocalPosition = rightArm.transform.localPosition;
            objectToGrab.transform.position = armLocalPosition;
        }*/
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), Color.green,);
        if (Input.GetButtonDown("GrabObject") && !grabbingAnObject)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, minDistance, layer))
            {
                //print("raycast OK");                
                grabbingAnObject = true;
                objectToGrab = hit.transform.gameObject;
                objectToGrab.GetComponent<Rigidbody>().useGravity = false;                
                StartCoroutine("IsTrigger");                
                objectToGrab.transform.parent = Camera.main.transform;
                ArmsAnimatorBehabior.GrabObjects(1);
            }
        }

        if (grabbingAnObject)
        {
            //print("cargando objeto");
            if (Input.GetButtonDown("ThrowObject")) //click alt -> lanzar
            {
                ThrowThatWeirdStuff();                                
            }
            else if (Input.GetButtonUp("GrabObject"))
            {
                //print("soltar");
                ArmsAnimatorBehabior.GrabObjects(0);
                objectToGrab.GetComponent<GrabbableObject>().ToTheGround();
            }
        }
        if (throwRequested) //temporizador básico para cuadrar el lanzamiento con la animación
        {            
            if (throwTemporizer <= 0)
            {                
                objectToGrab.GetComponent<GrabbableObject>().Throw();
                throwRequested = false;
            }
            else
            {
                throwTemporizer -= Time.deltaTime;
            }
        }        
    }

    IEnumerator IsTrigger()
    {
        objectToGrab.GetComponent<Rigidbody>().isKinematic = false;        
        yield return new WaitForSeconds(0.2f);
        objectToGrab.GetComponent<BoxCollider>().isTrigger = true;
        objectToGrab.GetComponent<Rigidbody>().isKinematic = true;
        objectToGrab.GetComponent<Rigidbody>().useGravity = true;
    }

    private void ThrowThatWeirdStuff()
    {        
        ArmsAnimatorBehabior.GrabObjects(2);
        throwTemporizer = 0.4f;
        throwRequested = true;        
    }    
}
