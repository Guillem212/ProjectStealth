using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualClimbSensor : MonoBehaviour
{
    GameObject player;

    // Use this for initialization
    void Awake()
    {
        player = GameObject.Find("Player"); //Cambiar
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Hookable") 
        {
            player.GetComponent<Climb>().setObjectToClimb(other.gameObject);
            Climb.readyToClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        if (other.tag == "Hookable")
        {
            Climb.readyToClimb = false;
        }
    }

}
