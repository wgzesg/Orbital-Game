using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationControl : MonoBehaviour
{
    Animator door_animator;
    // Start is called before the first frame update
    void Start()
    {
        door_animator = GetComponentInParent<Animator>();
        Debug.Log("Found animator");
    }

    
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("There is a collision");
        if(collision.GetComponents<Actor>() != null)
        {
            Debug.Log("valid collision");
            door_animator.SetBool("character_nearby", true);
        }
        else
        {
            Debug.Log("invalid collision");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponents<Actor>() != null)
        {
            door_animator.SetBool("character_nearby", false);
        }
    }
}
