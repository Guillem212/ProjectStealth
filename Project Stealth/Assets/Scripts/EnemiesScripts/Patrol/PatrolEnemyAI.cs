using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum EnemyState
{
    PATROL,
    SEARCHING,
    ATTACKING
}


public class PatrolEnemyAI : MonoBehaviour
{


    [HideInInspector]
    public EnemyState enemyState;

    public  EnemyState getEnemyState()
    {
        return enemyState;
    }

    public void setEnemyState(EnemyState state)
    {
        enemyState = state;
    }

    public NavMeshAgent agent;

    [HideInInspector]
    public GameObject playerPostion;
    
    private Image image;

    public Sprite imageStart;

    private Transform[] patrolPoints;
    public GameObject waypointEnemy;
    private int actualPoint = 0;

    public Light spot;

    private bool enemyShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyState = EnemyState.PATROL;
        image = GetComponentInChildren<Image>();
        playerPostion = GameObject.FindGameObjectWithTag("Player").gameObject;

        patrolPoints = waypointEnemy.GetComponentsInChildren<Transform>();
        actualPoint = 0;

        agent.stoppingDistance = 0;
        spot.colorTemperature = 20000;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyState.SEARCHING)
        {
            spot.colorTemperature = 3000;
            StartCoroutine(goToPosition());
        }
        else if(enemyState == EnemyState.ATTACKING)
        {
            agent.SetDestination(playerPostion.transform.position);
            spot.colorTemperature = 1000;
            agent.stoppingDistance = 6;
            //shoot();
         
        }
        else
        {
            spot.colorTemperature = 20000;
            if (agent.remainingDistance <= agent.stoppingDistance)
                actualPoint++;

            if (actualPoint > patrolPoints.Length - 1)
                actualPoint = 0;

            agent.SetDestination(patrolPoints[actualPoint].position);

        }
    }

    IEnumerator goToPosition()
    {
        image.sprite = imageStart;
        yield return new WaitForSeconds(1f);
        agent.SetDestination(StealthBehaviour.lastpositionKnown);
    }

}
