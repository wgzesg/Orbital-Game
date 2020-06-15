using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StationTriggerManager : MonoBehaviour
{
    public UnityAction OnEnteredStation;

    public UnityAction OnStayedStation;

    public UnityAction OnExitedStation;

    private void OnTriggerEnter(Collider other)

    {
        print("Station Ready to Use");
        if(OnEnteredStation != null)
        {
            OnEnteredStation(); // the station perform an animation to be activated
        }
    }
    private void OnTriggerStay(Collider player)
    {
        print("What do you want for Today");
        if (OnStayedStation != null)
        {
            OnStayedStation(); // the station show a hint, asking player to press 'E' to open the UI
        }
    }

    private void OnTriggerExit(Collider player)
    {
        print("GoodBye, Have a Nice Day");
        if(OnExitedStation != null)
        {
            OnExitedStation(); // the UI closed, and station perform animation to be deactivated.
        }
    }
}
