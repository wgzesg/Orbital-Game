using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 0f;
    public float movementSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime, ForceMode.Force);

        if (Input.GetKey("a"))
        {
            rb.AddForce(-movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(movementSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
    }
}
