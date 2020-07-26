using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneButton : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public string sceneName = "";

    public void LoadTargetScene()
    {
        Debug.Log("Loading to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void netLoadTargetScene()
    {
        Debug.Log("The current state is " + PhotonNetwork.NetworkClientState);
        if (PhotonNetwork.CurrentRoom != null)
        {
            Debug.Log("I am in room.");
            PhotonNetwork.LeaveRoom();
            StartCoroutine(customLeaveRoom());
        }
        else
        {
            LoadTargetScene();
        }
    }

    IEnumerator customLeaveRoom()
    {
        while(PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer)
        {
            Debug.Log("The current state is " + PhotonNetwork.NetworkClientState);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("The current state is " + PhotonNetwork.NetworkClientState);
        LoadTargetScene();
    }
}
