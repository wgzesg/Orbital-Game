using UnityEngine;

public class DoorAnimationV2 : MonoBehaviour
{
    Animator door_animator;
    // Start is called before the first frame update
    void Start()
    {
        door_animator = GetComponentInParent<Animator>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" ||collision.tag == "Enemy")
        {
            
            door_animator.SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            door_animator.SetBool("character_nearby", false);
        }
    }
}
