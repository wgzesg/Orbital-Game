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
    }

    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponents<Actor>() != null)
        {
            door_animator.SetBool("character_nearby", true);
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
