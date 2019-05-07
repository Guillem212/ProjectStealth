using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealhDetectorBeheaviour : MonoBehaviour
{

    private SphereCollider collider;
    public CharacterController player;

    private float lastVelY;


    private float hookNoise = 1.7f;
    private float cameraNoise = 1.5f;

    public float lerpVelocity = 2;

    private float initialRadius;

    private float noiseMultiplier;
    private float lightMultiplier;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        initialRadius = collider.radius;
        lightMultiplier = collider.radius;
    }

    // Update is called once per frame
    void Update()
    {
        noiseMultiplier = Mathf.Abs((player.velocity.x + player.velocity.z) / 1.5f);

        if (noiseMultiplier != 0)
            collider.radius = Mathf.Lerp(collider.radius, noiseMultiplier, lerpVelocity * Time.deltaTime);
        else
            collider.radius = Mathf.Lerp(collider.radius, initialRadius, lerpVelocity * Time.deltaTime);

        lastVelY = player.velocity.y;

        if(player.velocity.y > 0)
        {
            //comprobar cuando toca el suelo y aplicar ruido
        }
        //comporbar cuando lanzas el gancho
        //comprobar cuando lanzas la camara
    }

    public float GetNoise()
    {
        return noiseMultiplier;
    }

    public float GetLight()
    {
        return lightMultiplier;
    }


}
