using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsAnimatorBehabior : MonoBehaviour
{
    public GameObject g_rightArm;
    public GameObject g_leftArm;
    public static Animator rightArmAnimator;
    public static Animator leftArmAnimator;
    GameObject leftArmCollider;
    GameObject rightArmCollider;

    // Start is called before the first frame update
    void Start()
    {
        rightArmAnimator = g_rightArm.GetComponent<Animator>();
        leftArmAnimator = g_leftArm.GetComponent<Animator>();
        rightArmCollider = GameObject.Find("RightArmCollider");
        leftArmCollider = GameObject.Find("LeftArmCollider");
        leftArmAnimator.SetBool("Crouched", true);
        rightArmAnimator.SetBool("Crouched", true);
    }


    public static void ArmToWall(string name, bool condition)
    {
        if (name == "LeftArmCollider")
        {
            leftArmAnimator.SetBool("ToWall", condition);
        }
        else
        {
            rightArmAnimator.SetBool("ToWall", condition);
        }
    }

    public static void GrabObjects(int state)
    {
        switch (state)
        {
            case 1: //coger objeto
                {
                    rightArmAnimator.SetTrigger("GrabTrigger");
                    rightArmAnimator.SetBool("GrabbingAnObject", true);
                    break;
                }
            case 2: //lanzarlo
                {
                    rightArmAnimator.SetTrigger("ThrowObject");
                    rightArmAnimator.SetBool("GrabbingAnObject", false);
                    break;
                }
            default: //soltarlo
                {
                    rightArmAnimator.SetBool("GrabbingAnObject", false);
                    break;
                }
        }
    }

    public void ShowHands(bool enabled)
    {
        if (enabled)
        {
            //activa los colliders
            rightArmCollider.SetActive(true);
            leftArmCollider.SetActive(true);
            //saca las manos
            rightArmAnimator.SetBool("Crouched", true);
            leftArmAnimator.SetBool("Crouched", true);
        }
        else
        {
            //desactiva los colliders
            rightArmCollider.SetActive(false);
            leftArmCollider.SetActive(false);
            //guarda las manos
            rightArmAnimator.SetBool("Crouched", false);
            leftArmAnimator.SetBool("Crouched", false);
        }
    }

    public void SteathWalk(bool state)
    {
        if (state)
        {
            rightArmAnimator.SetBool("StealthWalking", true);
            leftArmAnimator.SetBool("StealthWalking", true);
        }
        else
        {
            rightArmAnimator.SetBool("StealthWalking", false);
            leftArmAnimator.SetBool("StealthWalking", false);
        }
    }

    public void SetActiveColliders(bool state) //para evitar choque de pared al trepar
    {
        rightArmCollider.SetActive(state);
        leftArmCollider.SetActive(state);
    }

    public static void ArmsToWall(bool condition)
    {
        leftArmAnimator.SetBool("ToWall", condition);
        rightArmAnimator.SetBool("ToWall", condition);
    }
}
