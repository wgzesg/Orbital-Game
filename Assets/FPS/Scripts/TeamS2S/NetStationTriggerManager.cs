using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class NetStationTriggerManager: StationTriggerManager
{
    PlayerAvatar localPlayer;

    private void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();
    }

    public override void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            if (OnEnteredStation != null)
            {
                OnEnteredStation.Invoke(); // the station perform an animation to be activated
            }
        }
    }
    public override void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            if (OnStayedStation != null)
            {
                OnStayedStation.Invoke(); // the station show a hint, asking player to press 'G' to open the UI
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            if (OnExitedStation != null)
            {
                OnExitedStation.Invoke(); // the UI closed, and station perform animation to be deactivated.
            }
        }
    }
}
