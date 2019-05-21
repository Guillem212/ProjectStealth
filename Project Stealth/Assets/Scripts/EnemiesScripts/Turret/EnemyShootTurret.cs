using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootTurret : MonoBehaviour
{
    private TurretEnemyAI turretAI;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if(turretAI.getEnemyState() != EnemyState.SEARCHING)
        {
            
        }
        
    }
}
