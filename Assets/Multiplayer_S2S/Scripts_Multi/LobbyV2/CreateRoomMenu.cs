using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;


    private RoomCanvases _roomCanvases;
    public void FirstInitialize(RoomCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        //CreateRoom
        //JoinOrCreateRoom
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
        _roomCanvases.CurrentRoomCanvas.Show();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation failed" + message, this);
    }
}