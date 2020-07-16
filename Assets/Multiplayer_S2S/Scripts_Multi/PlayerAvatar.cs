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

    public UnityAction PlayerSpawned;
    public UnityAction PlayerDied;



    private void Awake()
    {
        deathCam.gameObject.SetActive(false);
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            SpwanPlayer();
        }
    }

    public void OnDieHandler(GameObject damagesource)
    {
        isAlive = false;
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(playerAvatar);
            PlayerManager.PMinstance.OnDiedHandler(PV.ViewID);
            deathCam.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void deathHandlerRPC()
    {
        Destroy(playerAvatar);
        isAlive = false;
        PlayerManager.PMinstance.OnDiedHandler(PV.ViewID);
    }

    public void SpwanPlayer()
    {
        deathCam.gameObject.SetActive(false);
        playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), GameSetup.GS.playerBirthPlace[0].position, Quaternion.identity, 0);
        isAlive = true;
        Health playerHealth = playerAvatar.GetComponent<Health>();
        playerHealth.onDie += OnDieHandler;
        PlayerManager.PMinstance.PV.RPC("RPC_RegisterPlayers", RpcTarget.AllBuffered, PV.ViewID);
        Debug.Log("I spawned player");

        if (PV.IsMine && PlayerSpawned != null)
        {
            PlayerSpawned.Invoke();
        }
    }
}
