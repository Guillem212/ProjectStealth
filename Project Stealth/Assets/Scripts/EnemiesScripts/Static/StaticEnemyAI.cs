using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class StaticEnemyAI : MonoBehaviour
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

    public Sprite imageSearch;
    public Sprite imageAttack;

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

            if(agent.remainingDistance <= 0)
            {
                enemyState = EnemyState.PATROL;
            }
        }
        else if(enemyState == EnemyState.ATTACKING)
        {
            agent.SetDestination(playerPostion.transform.position);
            spot.colorTemperature = 1000;
            agent.stoppingDistance = 6;
            image.sprite = imageAttack;
            //shoot();

        }
        else
        {
            spot.colorTemperature = 20000;
        }
    }

    IEnumerator goToPosition()
    {
        image.CrossFadeAlpha(1, 1, true);
        image.sprite = imageSearch;
        yield return new WaitForSeconds(1f);
        agent.SetDestination(StealthBehaviour.lastpositionKnown);
    }

}
