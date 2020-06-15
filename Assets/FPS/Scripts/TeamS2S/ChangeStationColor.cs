using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStationColor : MonoBehaviour
{
    Renderer rend;
    StationTriggerManager station;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        station = GetComponentInChildren<StationTriggerManager>();
        station.OnEnteredStation += TurnRed;
        station.OnExitedStation += TurnBlack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnRed()
    {
        //GetComponent<Renderer>().material.color = Color.red;
        rend.materials[1].color = Color.red;
    }
    void TurnBlack()
    {
        //GetComponent<Renderer>().material.color = Color.black;
        rend.materials[1].color = Color.black;
    }
}
