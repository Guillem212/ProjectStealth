using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor.VFX;

public class PlayerShipParticleBehavior : MonoBehaviour
{
    public UnityEngine.Experimental.VFX.VisualEffect forceParticles;
    public GameObject areaLight1;
    public GameObject areaLight2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            forceParticles.SetFloat("bigOnes", 0.5f);
            forceParticles.SetFloat("smallOnes", 0.5f);
            areaLight1.SetActive(true);
            areaLight2.SetActive(true);
        }
    }
}
