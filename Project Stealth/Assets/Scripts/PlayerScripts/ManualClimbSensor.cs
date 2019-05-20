using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualClimbSensor : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Scalable") 
        {
            player.GetComponent<Climb>().setObjectToClimb(other.gameObject);
            Climb.readyToClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        if (other.tag == "Scalable")
        {
            Climb.readyToClimb = false;
        }
    }

}
