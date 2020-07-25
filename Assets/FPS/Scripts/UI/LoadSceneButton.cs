using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public string sceneName = "";

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void netLoadTargetScene()
    {
        Debug.Log("The current state is " + PhotonNetwork.NetworkClientState);
        if (PhotonNetwork.CurrentRoom != null)
        {
            Debug.Log("I am in room.");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            LoadTargetScene();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("The current state is " + PhotonNetwork.NetworkClientState);
        base.OnLeftRoom();
        LoadTargetScene();

    }
}
