using UnityEngine;

public class NetWorldspaceHealthBar : WorldspaceHealthBar
{
    PlayerAvatar localPlayer;

    public override void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();
        localPlayer.PlayerSpawned += OnPlayerSpawnHandler;
        localPlayer.PlayerDied += OnPlayerDiedHandler;
        main = localPlayer.playerAvatar.GetComponentInChildren<Camera>();
    }

    public void OnPlayerSpawnHandler()
    {
        main = localPlayer.playerAvatar.GetComponentInChildren<Camera>();
    }

    public void OnPlayerDiedHandler()
    {
        main = null;
    }

    public override void Update()
    {
        if (main == null)
        {
            return;
        }
            
        base.Update();
    }
}
