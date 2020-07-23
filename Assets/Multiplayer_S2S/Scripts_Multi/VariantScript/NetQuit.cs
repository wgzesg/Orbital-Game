using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetQuit : MonoBehaviour
{
    public void LeavePlayer()
    {
        StartCoroutine(LeaveAndLoad());
    }

    IEnumerator LeaveAndLoad()
    {
        Debug.Log("try to leave room...");
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        Debug.Log("leaveroom finished, load to menuScene ...");
        SceneManager.LoadScene(MultiplayerSettingV2.multiplayerSettingV2.menuScene);
    }
}
