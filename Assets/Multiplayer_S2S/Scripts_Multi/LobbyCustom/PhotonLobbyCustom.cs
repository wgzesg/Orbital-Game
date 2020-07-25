using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobbyCustom : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobbyCustom lobby; // singletons

    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPenal;


    private void Awake()
    {
        lobby = this; // reference instance of this class
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // connect to Master photon server.
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true; // change scene in the same way, same time
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();
        foreach(RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    void RemoveRoomListings()
    {
        while(roomsPenal.childCount != 0)
        {
            Destroy(roomsPenal.GetChild(0).gameObject);// continue to remove the first childe until all children are removed
        }
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPenal);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.SetRoom();
        }
    }


    public void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("we are now in a room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed. There must already be a room with the same name");
    }

    public void OnRoomNameChanged(string NameIn)
    {
        roomName = NameIn;
    }
    public void OnRoomSizeChanged(string SizeIn)
    {
        roomSize = int.Parse(SizeIn);
    }

    public void OnClick_JoinLobby()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}

