using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthBehaviour : MonoBehaviour
{
    public PlayerMovement player;

    private SphereCollider col;

    float radius;
    // Start is called before the first frame update
    void Start()
    {
        col = GameObject.Find("StealthIndex").GetComponent<SphereCollider>();
        radius = col.radius;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isWalking)
        {
            col.radius = Mathf.Lerp(col.radius, 4, Time.deltaTime * 2);
        }
        else
        {
            col.radius = Mathf.Lerp(col.radius, radius, Time.deltaTime * 4);
        }
    }
}
