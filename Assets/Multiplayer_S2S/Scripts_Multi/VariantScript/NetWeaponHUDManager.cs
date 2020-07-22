public class NetWeaponHUDManager: WeaponHUDManager
{
    public PlayerAvatar localPlayer;

    public override void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();
        localPlayer.PlayerSpawned += OnSpawnedHandler;
        OnSpawnedHandler();
    }

    public void OnSpawnedHandler()
    {
        m_PlayerWeaponsManager = localPlayer.playerAvatar.GetComponent<NetPlayerWeaponsManager>();

        ClearPanel();
        WeaponController activeWeapon = m_PlayerWeaponsManager.GetActiveWeapon();
        if (activeWeapon)
        {
            AddWeapon(activeWeapon, m_PlayerWeaponsManager.activeWeaponIndex);
            ChangeWeapon(activeWeapon);
        }

        m_PlayerWeaponsManager.onAddedWeapon += AddWeapon;
        m_PlayerWeaponsManager.onRemovedWeapon += RemoveWeapon;
        m_PlayerWeaponsManager.onSwitchedToWeapon += ChangeWeapon;
    }
}
