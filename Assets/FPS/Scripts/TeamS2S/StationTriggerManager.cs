using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StationTriggerManager : MonoBehaviour
{
    public UnityAction OnEnteredStation;

    public UnityAction OnStayedStation;

    public UnityAction OnExitedStation;

    public Boolean checkGButton = false;

    private void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player")
        {
            if (OnEnteredStation != null)
            {
                OnEnteredStation(); // the station perform an animation to be activated
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnStayedStation != null)
            {
                OnStayedStation(); // the station show a hint, asking player to press 'E' to open the UI
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnExitedStation != null)
            {
                OnExitedStation(); // the UI closed, and station perform animation to be deactivated.
            }
        }
    }
}
