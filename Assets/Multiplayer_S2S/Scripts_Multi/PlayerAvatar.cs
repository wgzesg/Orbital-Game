using System.IO;
using Photon.Pun;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    PhotonView PV;
    GameObject playerAvatar;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkAvatar"), GameSetup.GS.playerBirthPlace[0].position, Quaternion.identity, 0);
        }
        if (playerAvatar != null)
        {
            Debug.Log("The player is spawned");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
