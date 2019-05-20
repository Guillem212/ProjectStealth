using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbAnimationController : MonoBehaviour
{
    public GameObject player;
    public void EventOccured()
    {
        player.GetComponent<Climb>().EventOccured();
    }
}
