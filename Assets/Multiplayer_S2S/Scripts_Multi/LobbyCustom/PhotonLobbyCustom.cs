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

    public List<RoomInfo> roomLisitngs;


    private void Awake()
    {
        lobby = this; // reference instance of this class
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // connect to Master photon server.
        roomLisitngs = new List<RoomInfo>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true; // change scene in the same way, same time

        PhotonNetwork.NickName = "Human#" + Random.Range(0, 1000);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //RemoveRoomListings();
        int tempIndex;
        foreach(RoomInfo room in roomList)
        {
            if(roomLisitngs != null)
            {
                tempIndex = roomLisitngs.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1)
            {
                roomLisitngs.RemoveAt(tempIndex);
                Destroy(roomsPenal.GetChild(tempIndex).gameObject);
            }
            else
            {
                roomLisitngs.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void RemoveRoomListings()
    {
        for (int i = roomsPenal.childCount - 1; i >= 0; i--)  
        {
            Destroy(roomsPenal.GetChild(i).gameObject);
        }

        //alternative way:
        /*int i = 0;
        while(roomsPenal.childCount != 0)
        {
            Destroy(roomsPenal.GetChild(i).gameObject);
            i++;
        }*/
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

