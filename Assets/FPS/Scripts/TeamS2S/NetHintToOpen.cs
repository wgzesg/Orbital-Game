using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetHintToOpen : HintToOpen
{
    PlayerAvatar localPlayer;

    public override void Start()
    {
        station = GetComponent<StationTriggerManager>();

        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();
        localPlayer.PlayerDied += OnDiedHandler;
        localPlayer.PlayerSpawned += OnSpawnedHandler;
        canvasRoot.SetActive(false);

        OnSpawnedHandler();
    }

    public void OnSpawnedHandler()
    {
        m_weaponManager = localPlayer.playerAvatar.GetComponent<PlayerWeaponsManager>();
        station.OnEnteredStation += OnEnterHandler;
        station.OnExitedStation += OnExitHandler;
    }

    public void OnDiedHandler()
    {
        m_weaponManager = null;
        station.OnEnteredStation -= OnEnterHandler;
        station.OnExitedStation -= OnExitHandler;
    }
}
