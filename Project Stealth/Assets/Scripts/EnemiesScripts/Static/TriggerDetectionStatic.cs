using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionStatic : MonoBehaviour
{
    public float timeToStop = 3f;
    public StaticEnemyAI staticAI;

    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (staticAI.enemyState != EnemyState.ATTACKING)
            {
                staticAI.enemyState = EnemyState.ATTACKING;
                anim.SetTrigger("IsAttacking");

            }
        }
    }
}
