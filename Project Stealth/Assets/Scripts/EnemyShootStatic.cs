using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootStatic : MonoBehaviour
{
    private StaticEnemyAI staticAI;
    public LayerMask playerLayer;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        staticAI = GetComponent<StaticEnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if((staticAI.agent.remainingDistance <= staticAI.agent.stoppingDistance || !staticAI.agent.hasPath)
            && staticAI.getEnemyState() == EnemyState.ATTACKING)
        {
            transform.LookAt(staticAI.playerPostion.transform.position);

            if (timer > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                        playerAtt.life.TrySpendLife(0.5f);

                    }
                        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                }

                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0.5f;
            }
        }
        
    }
}
