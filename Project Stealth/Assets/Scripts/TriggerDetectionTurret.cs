using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetectionTurret : MonoBehaviour
{
    public float timeToStop = 3f;
    public TurretEnemyAI turretAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (turretAI.getEnemyState() != EnemyState.ATTACKING)
            {
                turretAI.setEnemyState(EnemyState.ATTACKING);
            }
        }
    }
}
