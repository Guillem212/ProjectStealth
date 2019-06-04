using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private PatrolEnemyAI patrolAI;
    public LayerMask playerLayer;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    public ParticleSystem[] particles;
    public bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        patrolAI = GetComponent<PatrolEnemyAI>();

        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if ((patrolAI.agent.remainingDistance <= patrolAI.agent.stoppingDistance || !patrolAI.agent.hasPath)
            && patrolAI.getEnemyState() == EnemyState.ATTACKING && !PauseManager.gameIsFinished)
        {
            transform.LookAt(patrolAI.playerPostion.transform.position);

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
        FindObjectOfType<AudioManager>().Play("r_shoot");
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        playerAtt.life.TrySpendLife(1f);
        isShooting = false;
    }
}
