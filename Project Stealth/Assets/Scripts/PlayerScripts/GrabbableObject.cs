using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor.VFX;

public class GrabbableObject : MonoBehaviour
{
    Rigidbody rigidBody;
    GrabObjects grabScript;
    [SerializeField] GameObject player;
    [SerializeField] UnityEngine.Experimental.VFX.VisualEffect forceParticles;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        grabScript = player.GetComponent<GrabObjects>();
        forceParticles = GetComponentInChildren<UnityEngine.Experimental.VFX.VisualEffect>();
        forceParticles.SetFloat("Input", 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grabScript.grabbingAnObject && !other.CompareTag("Player") && !other.CompareTag("Detector"))
        {
            print("collided with :" + other.name);
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
        FindObjectOfType<AudioManager>().Stop("forceField");
        grabScript.DeactivateParticles();
        forceParticles.SetFloat("Input", 0f);
        ArmsAnimatorBehabior.GrabObjects(0);
        rigidBody.isKinematic = false;
        grabScript.grabbingAnObject = false;
        transform.parent = null;
        GetComponent<BoxCollider>().isTrigger = false;
    }

    public void ActivateVFX()
    {
        forceParticles.SetFloat("Input", 0.5f);
    }

}
