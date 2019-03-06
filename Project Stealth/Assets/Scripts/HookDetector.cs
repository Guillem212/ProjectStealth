using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDetector : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Awake () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hookable")
        {
            GrappingHook.hooked = true;
            player.GetComponent<GrappingHook>().hookedObject = other.gameObject;
        }
    }
}
