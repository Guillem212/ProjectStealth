using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hookable")
        {
            GrappingHook.HookedIntoAnObject = true;
        }
    }   
}
