using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private PatrolEnemyAI patrolAI;
    public LayerMask playerLayer;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    public GameObject leftShoot;
    public GameObject rightShoot;

    // Start is called before the first frame update
    void Start()
    {
        patrolAI = GetComponent<PatrolEnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if((patrolAI.agent.remainingDistance <= patrolAI.agent.stoppingDistance || !patrolAI.agent.hasPath)
            && patrolAI.getEnemyState() == EnemyState.ATTACKING)
        {
            transform.LookAt(patrolAI.playerPostion.transform.position);

            if (timer > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                        playerAtt.life.TrySpendLife(0.5f);

                    }
                    else
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
                }

                rightShoot.SetActive(true);
                leftShoot.SetActive(true);


                timer -= Time.deltaTime;
            }
            else
            {

                rightShoot.SetActive(false);
                leftShoot.SetActive(false);
                timer = .1f;
            }
        }
        
    }
}
