﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gearScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<gearpointSystem>().gearpoints++;//add 1 to point system
            Destroy(gameObject); // this destroy the object.
        }
    }
}
