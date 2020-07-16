using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager PMinstance;
    public List<PlayerAvatar> playerList;
    public PhotonView PV;

    public int reviveTime = 3;

    public UnityAction OnAllDied;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PMinstance == null)
        {
            PMinstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(PMinstance.gameObject);
        }
        playerList = new List<PlayerAvatar>();
    }

    public PlayerAvatar findLocalPlayerAvatar()
    {
        foreach (PlayerAvatar p in playerList)
        {
            Debug.Log("Looking!!!");
            if (p.PV.IsMine)
            {
                Debug.Log("Found!!!");
                return p;
            }
        }
        Debug.Log("No mine!!!");
        return null;
    }

    public PlayerAvatar FindAnotherPlayer()
    {
        foreach (PlayerAvatar p in playerList)
        {
            Debug.Log("Looking for another player!!!");
            if (!p.PV.IsMine)
            {
                Debug.Log("Found another player!!!");
                return p;
            }
        }
        Debug.Log("No another player!!!");
        return null;
    }

    [PunRPC]
    public void RPC_RegisterPlayers(int PVID)
    {
        Debug.Log("I registered a new player");
        GameObject newPlayer = PhotonView.Find(PVID).gameObject;
        playerList.Add(newPlayer.GetComponent<PlayerAvatar>());
    }

    [PunRPC]
    public void RPC_Deregister(int PVID)
    {
        Debug.Log("I dereg an player");
        foreach(PlayerAvatar p in playerList)
        {
            if(p.PV.ViewID == PVID)
            {
                playerList.Remove(p);
            }
        }
    }

    public void OnDiedHandler(int deadPlayer)
    {
        if (CheckAllDied())
        {
            if (OnAllDied != null)
            {
                OnAllDied.Invoke();
            }
        }
        else
        {
            GameObject player = PhotonView.Find(deadPlayer).gameObject;
            if(player.GetPhotonView().IsMine)
                StartCoroutine(ReviveCoroutine(player));
        }
    }

    public bool CheckAllDied()
    {
        //foreach(PlayerAvatar p in playerList)
        //{
        //    if(p.isAlive == true)
        //    {
        //        return false;
        //    }
        //}
        return false;
    }

    IEnumerator ReviveCoroutine(GameObject player)
    {
        yield return new WaitForSeconds(reviveTime);

        player.GetComponent<PlayerAvatar>().SpwanPlayer();
    }
}
