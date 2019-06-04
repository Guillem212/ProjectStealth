﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootStatic : MonoBehaviour
{
    private StaticEnemyAI staticAI;
    public LayerMask playerLayer;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    public ParticleSystem[] particles;

    public bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        staticAI = GetComponent<StaticEnemyAI>();

        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if ((staticAI.agent.remainingDistance <= staticAI.agent.stoppingDistance || !staticAI.agent.hasPath)
            && staticAI.getEnemyState() == EnemyState.ATTACKING)
        {
            transform.LookAt(staticAI.playerPostion.transform.position);

            if (timer > 0)
            {
                if (!isShooting)
                {
                    StartCoroutine(shootReal());
                    isShooting = true;
                }

                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0.5f;
            }

        }
    }


    IEnumerator shootReal()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        playerAtt.life.TrySpendLife(0.5f);
        isShooting = false;
    }
}
