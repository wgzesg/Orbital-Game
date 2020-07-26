using System.Collections;
using Photon.Pun;
using UnityEngine;

public class NetGameFlowManager: GameFlowManager
{
    int nextLevel;
    public PhotonView PV;

    public override void Start()
    {
        m_ObjectiveManager = FindObjectOfType<ObjectiveManager>();
        DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, GameFlowManager>(m_ObjectiveManager, this);
        m_ObjectiveManager.OnAllCompleted += OnAllcompletedHandler;

        PlayerManager.PMinstance.OnAllDied += OnAllDiedHandler;

        PV = GetComponent<PhotonView>();

        AudioUtility.SetMasterVolume(1);
    }

    public void OnAllDiedHandler()
    {
        EndGame(false);
    }

    public override IEnumerator fading()
    {
        while(Time.time <= m_TimeLoadEndGameScene)
        {
            float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;

            AudioUtility.SetMasterVolume(1 - timeRatio);
            yield return new WaitForEndOfFrame();
        }

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(nextLevel);

        gameIsEnding = false;
    }

    public override void EndGame(bool win)
    {
        // unlocks the cursor before leaving the scene, to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Endgame is run");

        PV.RPC("scoreUpdate", RpcTarget.AllBuffered);

        // Remember that we need to load the appropriate end scene after a delay
        endGameFadeCanvasGroup.gameObject.SetActive(true);
        if (win)
        {
            Debug.Log("The win part of EndGame is run");
            nextLevel = MultiplayerSettingV2.multiplayerSettingV2.NetWinScene;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = victorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);

            // create a game message
            var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
            if (message)
            {
                message.delayBeforeShowing = delayBeforeWinMessage;
                message.GetComponent<Transform>().SetAsLastSibling();
            }
        }
        else
        {
            Debug.Log("The lose part of EndGame is run");
            nextLevel = MultiplayerSettingV2.multiplayerSettingV2.NetLoseScene;
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay;
        }

        gameIsEnding = true;
    }

    [PunRPC]
    public override void scoreUpdate()
    {
        base.scoreUpdate();
    }


}
