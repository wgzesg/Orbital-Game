using Photon.Pun;
using UnityEngine;

public class NetinventoryHUD: inventoryHUD
{
    public PlayerAvatar localPlayer;
    public override void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();
        localPlayer.PlayerDied += OnDiedHandler;
        localPlayer.PlayerSpawned += OnSpawnedHandler;
        OnSpawnedHandler();
    }

    public void OnSpawnedHandler()
    {
        playerInventory = localPlayer.playerAvatar.GetComponent<Inventory>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, LoadHUD>(playerInventory, this);

        inventoryDisplay.text = "Gears: " + playerInventory.gearCount;

        playerInventory.onUpdateGearCount += OnChangedGearCount;
    }

    public void OnDiedHandler()
    {
        playerInventory = null;
    }
}
