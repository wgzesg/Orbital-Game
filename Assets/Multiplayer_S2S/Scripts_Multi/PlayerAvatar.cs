using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAvatar : MonoBehaviour
{
    public PhotonView PV;
    public GameObject playerAvatar;
    public bool isAlive;

    public UnityAction PlayerSpawned;
    public UnityAction PlayerDied;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        SpwanPlayer();

        PlayerManager.PMinstance.RegisterPlayers(PV.ViewID);
    }

    public void OnDieHandler(GameObject damagesource)
    {
        PV.RPC("deathHandlerRPC", RpcTarget.All);
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
        if (PV.IsMine)
        {
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), GameSetup.GS.playerBirthPlace[0].position, Quaternion.identity, 0);
            isAlive = true;
            Health playerHealth = playerAvatar.GetComponent<Health>();
            playerHealth.onDie += OnDieHandler;

            if (PV.IsMine && PlayerSpawned != null)
            {
                PlayerSpawned.Invoke();
            }
        }
    }
}
