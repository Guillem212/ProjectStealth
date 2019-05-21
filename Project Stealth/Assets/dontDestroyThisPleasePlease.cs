using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyThisPleasePlease : MonoBehaviour
{
    private GameObject instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
