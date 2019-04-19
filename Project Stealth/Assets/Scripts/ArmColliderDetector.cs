using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmColliderDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Wall")
        {
            ArmsAnimatorBehabior.ArmToWall(this.name, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            ArmsAnimatorBehabior.ArmToWall(this.name, false);
        }
    }
}
