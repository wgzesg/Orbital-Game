using UnityEngine;
using UnityEngine.UI;

public class NetLoadHUD: LoadHUD
{
    PlayerAvatar localPlayer;

    public override void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();

        localPlayer.PlayerDied += OnDiedHandler;
        localPlayer.PlayerSpawned += OnSpawnedHandler;
        OnSpawnedHandler();
    }

    public void OnSpawnedHandler()
    {
        m_playerWeaponsManager = localPlayer.playerAvatar.GetComponent<PlayerWeaponsManager>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, LoadHUD>(m_playerWeaponsManager, this);

        curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();

        m_playerWeaponsManager.onSwitchedToWeapon += OnswitchWeaponHandler;
    }

    public void OnDiedHandler()
    {
        curr_activeWeapon = null;
    }

    public override void Update()
    {
        if (curr_activeWeapon == null)
        {
            Debug.Log("it is null");
            return;
        }

        base.Update();
    }
}

