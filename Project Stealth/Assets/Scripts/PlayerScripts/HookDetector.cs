using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hook"))
        {
            GrappingHook.HookedIntoAnObject = true;
            FindObjectOfType<AudioManager>().Play("hookHit");
        }
    }
}
