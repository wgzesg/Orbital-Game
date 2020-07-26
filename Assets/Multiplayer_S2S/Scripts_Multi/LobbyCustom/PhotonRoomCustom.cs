using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoomCustom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomCustom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //player info
    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    //delay start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;
    public GameObject leaveButton;
    public GameObject MasterLeaveButton;

    private void Awake()
    {
        //set up singleton
        if (PhotonRoomCustom.room == null)
        {
            PhotonRoomCustom.room = this;
        }
        else
        {
            if (PhotonRoomCustom.room != this)
            {
                Destroy(PhotonRoomCustom.room.gameObject);
                PhotonRoomCustom.room = this;
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

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;// count down from 5
        timeToStart = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
            }
            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Display time to start to the players " + timeToStart);
                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("we are now in a room");
        PhotonNetwork.AutomaticallySyncScene = true;

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
            leaveButton.SetActive(false);
            MasterLeaveButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
            leaveButton.SetActive(true);
            MasterLeaveButton.SetActive(false);
        }

        ClearPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        //PhotonNetwork.NickName = myNumberInRoom.ToString(); // used in PhotonLobbyCustom.cs alr, no longer effective
        if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
        {
            Debug.Log("Display players in room out of max players possible (" + playersInRoom + ":" + MultiplayerSettingV2.multiplayerSettingV2.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettingV2.multiplayerSettingV2.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        /*else
        {
            StartGame();
        }*/
    }

    void ClearPlayerListings()
    {
        for(int i = playersPanel.childCount -1; i >= 0; i--)  
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room alr");
        //base.OnLeftRoom();
        PhotonNetwork.AutomaticallySyncScene = false;
        if(currentScene == MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene)
        {
            PhotonNetwork.LoadLevel(MultiplayerSettingV2.multiplayerSettingV2.menuScene);
        }
        else if (currentScene == MultiplayerSettingV2.multiplayerSettingV2.menuScene)
        {
            lobbyGO.SetActive(true);
            roomGO.SetActive(false);
        }
        
        
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        ClearPlayerListings();
        ListPlayers();


        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
        {
            Debug.Log("Display player in room out of max players possible (" + playersInRoom + ":" + MultiplayerSettingV2.multiplayerSettingV2.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSettingV2.multiplayerSettingV2.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
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

        }
        PhotonNetwork.LoadLevel(MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene);
    }

    public void OnClick_LeaveRoom()
    {
        Debug.Log("Start to leave room...");
        PhotonNetwork.LeaveRoom();
    }

    public void OnClick_MasterLeaveRoom()
    {
        Debug.Log("Master start to Leave room...");
        PhotonNetwork.LeaveRoom();
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene)
        {
            isGameLoaded = true;

            if (MultiplayerSettingV2.multiplayerSettingV2.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.All);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
        else if(currentScene == MultiplayerSettingV2.multiplayerSettingV2.menuScene)
        {
            Debug.Log("loaded to the lobby(menu) scene alr");
            isGameLoaded = false;
            lobbyGO.SetActive(true);
            roomGO.SetActive(false);
        }
        else if(currentScene == MultiplayerSettingV2.multiplayerSettingV2.mainManuScene)
        {
            Debug.Log("Back to MainMenu Now");
            PhotonNetwork.Disconnect();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            RPC_CreatePlayer();
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == MultiplayerSettingV2.multiplayerSettingV2.multiplayerScene)
        {
            SceneManager.LoadScene(MultiplayerSettingV2.multiplayerSettingV2.NetLoseScene);
        }
        else
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log(otherPlayer.NickName + " has left the game");
            playersInRoom--;

            ClearPlayerListings();
            ListPlayers();
        }
    }

}
