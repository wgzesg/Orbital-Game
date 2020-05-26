using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{

    public float speed = 4f;
    public float rotationSpeed = 15f;
    float gravity = 8;
    float rotation;
    Vector3 dir = Vector3.zero;

    CharacterController charControl;
    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        anime = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anime.SetInteger("State", 2);
            dir = new Vector3(0, 0, 1);
            dir *= speed;
            dir = transform.TransformDirection(dir);

        }
        else if (Input.GetKey(KeyCode.Space))
        {
            anime.SetInteger("State", 1);
            anime.SetBool("isJumping", true);
            dir = new Vector3(0, 1, 0);
            dir *= speed;
            dir = transform.TransformDirection(dir);
        }
        else
        {
            anime.SetInteger("State", 0);
            dir = Vector3.zero;
        }

        rotation += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);
        //dir.y -= gravity * Time.deltaTime;
        charControl.Move(dir * Time.deltaTime);

    }

}
