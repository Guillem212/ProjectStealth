using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor.VFX;

public class GrabObjects : MonoBehaviour
{

    [SerializeField] private float throwForce;
    [SerializeField] private float minDistance; //distancia minima hasta el objeto
    private GameObject objectToGrab;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject rightArm;    
    public UnityEngine.Experimental.VFX.VisualEffect forceParticles;

    Vector3 armLocalPosition;

    [HideInInspector] public bool grabbingAnObject = false;    

    float GetThrowForce() { return throwForce; }

    public float throwForce_P
    {
        get { return throwForce; }
        set { throwForce = value; }
    }

    void Update()
    {
        if (Input.GetButtonDown("GrabObject") && !grabbingAnObject)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, minDistance, layer))
            {                
                grabbingAnObject = true;                
                objectToGrab = hit.transform.gameObject;
                objectToGrab.GetComponent<GrabbableObject>().ActivateVFX();
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
                ArmsAnimatorBehabior.GrabObjects(2);
                DeactivateParticles();
            }
            else if (Input.GetButtonUp("GrabObject"))
            {
                //print("soltar");
                ArmsAnimatorBehabior.GrabObjects(0);
                objectToGrab.GetComponent<GrabbableObject>().ToTheGround();
                DeactivateParticles();
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

    public void ThrowThatWeirdStuff()
    {                
        objectToGrab.GetComponent<GrabbableObject>().Throw();
    }    

    public void DeactivateParticles()
    {        
        forceParticles.SetFloat("Input", 0f);
    }
}
