﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class StationTriggerManager : MonoBehaviour
{
    public UnityAction OnEnteredStation;

    public UnityAction OnStayedStation;

    public UnityAction OnExitedStation;

    public Boolean checkGButton = false;

    public virtual void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player")
        {
            if (OnEnteredStation != null)
            {
                OnEnteredStation.Invoke(); // the station perform an animation to be activated
            }
        }
    }
    public virtual void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnStayedStation != null)
            {
                OnStayedStation.Invoke(); // the station show a hint, asking player to press 'G' to open the UI
            }
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnExitedStation != null)
            {
                OnExitedStation.Invoke(); // the UI closed, and station perform animation to be deactivated.
            }
        }
    }
}
