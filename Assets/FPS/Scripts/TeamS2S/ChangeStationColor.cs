using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStationColor : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StationTriggerManager.OnEnteredStation += TurnRed;
        StationTriggerManager.OnExitedStation += TurnBlack;
    }
    private void OnDisable()
    {
        StationTriggerManager.OnEnteredStation -= TurnRed;
        StationTriggerManager.OnExitedStation -= TurnBlack;
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
