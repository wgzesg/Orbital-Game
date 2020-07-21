﻿using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAvatar : MonoBehaviour
{
    public PhotonView PV;
    public GameObject playerAvatar;
    public bool isAlive;
    public Camera deathCam;
    public GameObject playerBodyAvatar;

    public UnityAction  PlayerSpawned;
    public UnityAction PlayerDied;

    public Transform MyDeathPoint;
    public Transform MyRevivalPoint;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            spawnPlayer();
        }
    }

    public void OnDieHandler(GameObject damagesource)
    {
        if (PV.IsMine)
        {
            MyDeathPoint = playerAvatar.transform;

            isAlive = false;
            PhotonNetwork.Destroy(playerAvatar);
            PlayerManager.PMinstance.OnDiedHandler(PV.ViewID);
            deathCam.gameObject.SetActive(true);

            if (PlayerDied != null)
            {
                PlayerDied.Invoke();
            }
        }
    }

    public void spawnPlayer()
    {
        deathCam.gameObject.SetActive(false);
        PlayerManager.PMinstance.PV.RPC("RPC_RegisterPlayers", RpcTarget.AllBuffered, PV.ViewID);

        if (MyDeathPoint)
        {
            MyRevivalPoint = Determine_SpawnPoint(MyDeathPoint);
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), MyRevivalPoint.position, MyRevivalPoint.rotation);
        }
        else
        {
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), GameSetup.GS.playerBirthPlace[0].position, GameSetup.GS.playerBirthPlace[0].rotation);
        }
        Health playerHealth = playerAvatar.GetComponent<Health>();
        playerHealth.onDie += OnDieHandler;
        Debug.Log("I spawned player");

        if (PlayerSpawned != null)
        {
            PlayerSpawned.Invoke();
        }

        PV.RPC("RPC_SpawnPlayer", RpcTarget.AllBuffered, playerAvatar.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    public void RPC_SpawnPlayer(int viewID)
    {
        playerAvatar = PhotonView.Find(viewID).gameObject;
        deathCam.gameObject.SetActive(false);
        isAlive = true;
    }

    public Transform Determine_SpawnPoint(Transform dealthPoint)
    {
        if (dealthPoint.position.y >= 8.5)
        {
            return GameSetup.GS.playerBirthPlace[0];
        }
        else
        {
            return GameSetup.GS.playerBirthPlace[2];
        }
        
    }

}
