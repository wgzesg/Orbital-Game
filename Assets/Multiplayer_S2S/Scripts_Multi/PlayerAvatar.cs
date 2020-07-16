using System.IO;
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

    public UnityAction PlayerSpawned;
    public UnityAction PlayerDied;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            deathCam.gameObject.SetActive(false);
            PlayerManager.PMinstance.PV.RPC("RPC_RegisterPlayers", RpcTarget.AllBuffered, PV.ViewID);
            PV.RPC("RPC_SpawnPlayer", RpcTarget.All);
        }
    }

    public void OnDieHandler(GameObject damagesource)
    {
        if (PV.IsMine)
        {
            isAlive = false;
            PhotonNetwork.Destroy(playerAvatar);
            PlayerManager.PMinstance.OnDiedHandler(PV.ViewID);
            deathCam.gameObject.SetActive(true);
        }
    }

    public void RPC_SpawnPlayer()
    {
        deathCam.gameObject.SetActive(false);
        playerAvatar = Instantiate(playerBodyAvatar, GameSetup.GS.playerBirthPlace[0].position, Quaternion.identity);
        isAlive = true;
        Health playerHealth = playerAvatar.GetComponent<Health>();
        playerHealth.onDie += OnDieHandler;
        Debug.Log("I spawned player");

        if (PlayerSpawned != null)
        {
            PlayerSpawned.Invoke();
        }
    }
}
