using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationTriggerManager : MonoBehaviour
{
    public delegate void EnterStationAction();
    public static event EnterStationAction OnEnteredStation;

    public delegate void StayStationAction();
    public static event StayStationAction OnStayedStation;

    public delegate void ExitStationAction();
    public static event ExitStationAction OnExitedStation;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider player)
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
