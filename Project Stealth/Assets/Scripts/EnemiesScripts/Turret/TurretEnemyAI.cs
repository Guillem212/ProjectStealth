using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class TurretEnemyAI : MonoBehaviour
{


    [HideInInspector]
    private EnemyState enemyState;

    public GameObject playerPostion;

    public Light spot;

    private bool enemyShooting = false;

    private float startRotation;

    public PlayerAttributes playerAtt;

    private float timer = 0.5f;

    private bool rotatingRight = true;

    public GameObject leftShoot;
    public GameObject rightShoot;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.SEARCHING;

        playerPostion = GameObject.FindGameObjectWithTag("Player").gameObject;

        spot.colorTemperature = 20000;
        startRotation = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyState.SEARCHING)
        {
            spot.colorTemperature = 3000;

            if (transform.eulerAngles.y < startRotation + 30 && rotatingRight)
            {
                transform.Rotate(new Vector3(0, 30, 0), Time.deltaTime * 5);
            }
            else if(transform.eulerAngles.y > startRotation - 30)
            {
                rotatingRight = false;
                transform.Rotate(new Vector3(0, -30, 0), Time.deltaTime * 5);
            }
            else
            {
                rotatingRight = true;
            }

        }
        else if(enemyState == EnemyState.ATTACKING)
        {
            spot.colorTemperature = 1000;
            shoot();

        }
    }

    private void shoot()
    {
        transform.LookAt(playerPostion.transform.position);

        if (timer > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    playerAtt.life.TrySpendLife(0.5f);

                }
            }

            rightShoot.SetActive(true);
            leftShoot.SetActive(true);

            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0.5f;
            rightShoot.SetActive(false);
            leftShoot.SetActive(false);
        }
    }

    public EnemyState getEnemyState()
    {
        return enemyState;
    }

    public void setEnemyState(EnemyState state)
    {
        enemyState = state;
    }

}
