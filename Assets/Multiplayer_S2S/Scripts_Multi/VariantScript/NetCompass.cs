using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetCompass : Compass
{

    private PlayerAvatar PA;

    public override void Start()
    {
        PA = PlayerManager.PMinstance.findLocalPlayerAvatar();

        PA.PlayerSpawned += OnPlayerSpawnHandler;
        PA.PlayerDied += OnPlayerDiedHandler;
        m_PlayerTransform = PA.playerAvatar.transform;
    }

    public void OnPlayerSpawnHandler()
    {
        m_PlayerTransform = PA.playerAvatar.transform;
    }

    public void OnPlayerDiedHandler()
    {
        m_PlayerTransform = GameSetup.GS.playerBirthPlace[0];
    }
}
