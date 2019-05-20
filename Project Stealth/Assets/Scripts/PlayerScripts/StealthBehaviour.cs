using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthBehaviour : MonoBehaviour
{

    private SphereCollider colliderSphere;
    private float updateVel = 2f;

    private CharacterController charController;

    public static float amountOfSound = 0.5f;
    private float velocityOfPlayer = 0;


    private bool wasGrounded = true;
    public static Vector3 lastpositionKnown;

    // Start is called before the first frame update
    void Start()
    {
        colliderSphere = GetComponent<SphereCollider>();
        colliderSphere.radius = amountOfSound;
        charController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        updateCollider();
    }

    private void updateCollider()
    {
        velocityOfPlayer = (Mathf.Abs(charController.velocity.x) + Mathf.Abs(charController.velocity.z)) * 2 ;

        if (velocityOfPlayer != 0)
        {
            amountOfSound = Mathf.Abs(velocityOfPlayer);
        }
        else
        {
           amountOfSound = 0.5f;
        }

        if (charController.isGrounded && !wasGrounded)
        {
            amountOfSound += 100;
        }
        wasGrounded = charController.isGrounded;

        colliderSphere.radius = Mathf.Lerp(colliderSphere.radius, amountOfSound, Time.deltaTime * updateVel);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<PatrolEnemyAI>().getEnemyState() == EnemyState.PATROL)
            {
                lastpositionKnown = transform.position;
                other.gameObject.GetComponent<PatrolEnemyAI>().setEnemyState(EnemyState.SEARCHING);
                other.gameObject.GetComponent<PatrolEnemyAI>().agent.stoppingDistance = 2;
            }
            /*else if (other.gameObject.GetComponent<PatrolEnemyAI>().getEnemyState() == EnemyState.SEARCHING)
            {
                other.gameObject.GetComponent<PatrolEnemyAI>().setEnemyState(EnemyState.ATTACKING);
            }*/
        }
        else if (other.CompareTag("EnemyStatic"))
        {
            if (other.gameObject.GetComponent<StaticEnemyAI>().getEnemyState() == EnemyState.PATROL)
            {
                lastpositionKnown = transform.position;
                other.gameObject.GetComponent<StaticEnemyAI>().setEnemyState(EnemyState.SEARCHING);
                other.gameObject.GetComponent<StaticEnemyAI>().agent.stoppingDistance = 2;
            }
        }
    }
}
