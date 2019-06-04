using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{

    [HideInInspector]
    public bool isThrowed;

    public GameObject[] enemies;

    private void Start()
    {
        isThrowed = false;
        enemies = GameObject.FindGameObjectsWithTag("EnemyStatic");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isThrowed)
        {
            print("Hola");
            foreach (GameObject i in enemies)
            {
                print("holita");
                StaticEnemyAI.goToBottle = true;
                StaticEnemyAI.bottle = gameObject;
                i.GetComponent<StaticEnemyAI>().setEnemyState(EnemyState.SEARCHING);
                isThrowed = false;
            }
        }
    }
}
