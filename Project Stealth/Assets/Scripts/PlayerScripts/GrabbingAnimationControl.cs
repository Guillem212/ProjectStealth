using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingAnimationControl : MonoBehaviour
{
    public GameObject player;
    GrabObjects grabScript;


    private void Awake()
    {
        grabScript = player.GetComponent<GrabObjects>();
    }
    public void GrabStart()
    {
        if (grabScript.grabbingAnObject)
            //cuando levanta la mano activa las particulas
            player.GetComponent<GrabObjects>().forceParticles.SetFloat("Input", 0.5f);
    }
    public void ThrowRequested()
    {
        //sincronizador de animacion de lanzamiento
        player.GetComponent<GrabObjects>().ThrowThatWeirdStuff();
    }
}
