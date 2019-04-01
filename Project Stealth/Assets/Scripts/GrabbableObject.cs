using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    Rigidbody rigidBody;    
    GrabObjects grabScript;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        grabScript = GameObject.Find("PlayerAlvaro").GetComponent<GrabObjects>();
    }    
    
    private void OnTriggerEnter(Collider other)
    {
        if (grabScript.grabbingAnObject && other.tag != "Player")
        {         
            ToTheGround();
        }        
    }

    public void Throw()
    {        
        ToTheGround();
        rigidBody.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * grabScript.throwForce_P);
    }

    public void ToTheGround()
    {     
        rigidBody.isKinematic = false;
        grabScript.grabbingAnObject = false;
        transform.parent = null;
        GetComponent<BoxCollider>().isTrigger = false;
    }
}
