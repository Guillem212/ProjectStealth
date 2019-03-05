using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCamera : MonoBehaviour
{

    public GameObject camera;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            camera.SetActive(!camera.activeSelf);
        }
        
    }
}
