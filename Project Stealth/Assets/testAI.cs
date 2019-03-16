using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testAI : MonoBehaviour
{

    [HideInInspector]
    public NavMeshAgent agent;
    public GameObject player;

    public static bool detected;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        detected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (detected)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}
