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
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            deathCam.gameObject.SetActive(false);
            PlayerManager.PMinstance.PV.RPC("RPC_RegisterPlayers", RpcTarget.AllBuffered, PV.ViewID);
            SpwanPlayer();
        }
    }

    public void OnDieHandler(GameObject damagesource)
    {
        if (PV.IsMine)
        {
            isAlive = false;
            PhotonNetwork.Destroy(playerAvatar);
            deathCam.gameObject.SetActive(true);
            PlayerManager.PMinstance.OnDiedHandler(PV.ViewID);
        }
    }

    public void SpwanPlayer()
    {
        deathCam.gameObject.SetActive(false);
        playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), GameSetup.GS.playerBirthPlace[0].position, Quaternion.identity, 0);
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
