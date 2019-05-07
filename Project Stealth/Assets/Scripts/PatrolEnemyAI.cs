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


    public EnemyState enemyState;
    private NavMeshAgent agent;
    private Vector3 positionPlayer;
    public Image image;

    private Vector3 playerPos;

    public Sprite imageStart;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyState = EnemyState.PATROL;
        image = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyState.SEARCHING)
        {
            /*positionPlayer = Vector3.Lerp(transform.position, StealthBehaviour.lastpositionKnown, Time.deltaTime);
            transform.LookAt(positionPlayer);*/
            StartCoroutine(goToPosition());
        }
        else if(enemyState == EnemyState.ATTACKING)
        {
            agent.SetDestination(playerPos);
        }
    }

    IEnumerator goToPosition()
    {
        image.sprite = imageStart;
        yield return new WaitForSeconds(1f);
        agent.SetDestination(StealthBehaviour.lastpositionKnown);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            enemyState = EnemyState.ATTACKING;
        }
    }

}
