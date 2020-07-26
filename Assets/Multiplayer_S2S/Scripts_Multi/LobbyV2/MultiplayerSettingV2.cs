using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettingV2 : MonoBehaviour
{
    public static MultiplayerSettingV2 multiplayerSettingV2;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;
    public int singlePlayerScene;
    public int winScene;
    public int loseScene;

    public int NetWinScene;
    public int NetLoseScene;

    public int mainManuScene; // the starting scene when the application runs.

    private void Awake()
    {
        if (MultiplayerSettingV2.multiplayerSettingV2 == null)
        {
            MultiplayerSettingV2.multiplayerSettingV2 = this;
        }
        else
        {
            if (MultiplayerSettingV2.multiplayerSettingV2 != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
