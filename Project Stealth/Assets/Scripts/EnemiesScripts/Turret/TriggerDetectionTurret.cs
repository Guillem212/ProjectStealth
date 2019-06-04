using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionTurret : MonoBehaviour
{
    public float timeToStop = 3f;
    public TurretEnemyAI turretAI;

    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (turretAI.getEnemyState() != EnemyState.ATTACKING)
            {
                FindObjectOfType<AudioManager>().Play("r_detection");
                turretAI.setEnemyState(EnemyState.ATTACKING);
                anim.SetTrigger("IsAttacking");
            }
        }
    }
}
