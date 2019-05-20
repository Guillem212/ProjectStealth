using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public float timeToStop = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PatrolEnemyAI.enemyState != EnemyState.ATTACKING)
            {
                /*print("a ver si me detecta");
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (transform.position - other.transform.position).normalized, out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        print("detectado");*/
                        PatrolEnemyAI.enemyState = EnemyState.ATTACKING;
                   /* }
                    else
                    {
                        print("nope");
                    }
                }*/
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && PatrolEnemyAI.enemyState == EnemyState.ATTACKING)
        {
            StartCoroutine(timeToStopAttacking());
        }
    }

    IEnumerator timeToStopAttacking()
    {
        yield return new WaitForSeconds(timeToStop);
        PatrolEnemyAI.enemyState = EnemyState.SEARCHING;
    }*/
}
