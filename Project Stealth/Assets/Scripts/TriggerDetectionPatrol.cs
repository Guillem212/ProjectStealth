using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionPatrol : MonoBehaviour
{
    public float timeToStop = 3f;
    public PatrolEnemyAI patrolAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (patrolAI.enemyState != EnemyState.ATTACKING)
            {
                patrolAI.enemyState = EnemyState.ATTACKING;

            }
        }
    }
}
