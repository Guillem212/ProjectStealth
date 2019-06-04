
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
            foreach (GameObject i in enemies)
            {
                StaticEnemyAI.goToBottle = true;
                StaticEnemyAI.bottle = gameObject;
                i.GetComponent<StaticEnemyAI>().setEnemyState(EnemyState.SEARCHING);
                isThrowed = false;
            }
        }
    }
}
