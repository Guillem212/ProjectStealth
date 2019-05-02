using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    Rigidbody rigidBody;    
    GrabObjects grabScript;
    [SerializeField] GameObject player;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        grabScript = player.GetComponent<GrabObjects>();
    }    
    
    private void OnTriggerEnter(Collider other)
    {
        if (grabScript.grabbingAnObject && other.tag == "Wall")
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
        ArmsAnimatorBehabior.GrabObjects(0);
        rigidBody.isKinematic = false;
        grabScript.grabbingAnObject = false;
        transform.parent = null;
        GetComponent<BoxCollider>().isTrigger = false;
    }    

}
