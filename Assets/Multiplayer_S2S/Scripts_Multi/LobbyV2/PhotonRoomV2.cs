using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoomV2 : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomV2 roomV2;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    public int playerInGame;

    private void Awake()
    {
        if(PhotonRoomV2.roomV2 == null)
        {
            PhotonRoomV2.roomV2 = this;
        }
        else
        {
            if(PhotonRoomV2.roomV2 != this)
            {
                Destroy(PhotonRoomV2.roomV2.gameObject);
                PhotonRoomV2.roomV2 = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

        }
        PhotonNetwork.LoadLevel(MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene);
    }


    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene)
        {
            isGameLoaded = true;

            if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
            {
                RPC_CreatePlayer();
                //PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
        else if(currentScene == MultiplayerSettingV2.multiplayerSettingV2.menuScene){
            Debug.Log("Load to the menu scene alr");
            //PhotonNetwork.Disconnect();
            if(PhotonNetwork.IsConnected)
                DisconnectLocalClient();
            
        }
    }


    public void DisconnectLocalClient()
    {
        StartCoroutine(DisconnectFromMaster());
    }

    IEnumerator DisconnectFromMaster()
    {
        Debug.Log("now disconnect from server ...");
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        Debug.Log("local player has been disconnected");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) // for remote client
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the room");
        playerInGame--;
    }

    public override void OnLeftRoom() // for local client
    {
        base.OnLeftRoom();
        Debug.Log("you have left the room");
    }



    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    //[PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}
