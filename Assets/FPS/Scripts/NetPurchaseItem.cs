using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetPurchaseItem: PurchaseItem
{
    PlayerAvatar localplayer;
    public override void Start()
    {

        localplayer = PlayerManager.PMinstance.findLocalPlayerAvatar();

        localplayer.PlayerSpawned += OnSpawnedHandler;
        localplayer.PlayerDied += OnDiedHandler;

        OnSpawnedHandler();
        m_itemDisplay = GetComponent<ItemDisplay>();
        buttonText = purchaseKey.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        countDown.gameObject.SetActive(false);

    }

    public void OnSpawnedHandler()
    {
        m_weaponManager = localplayer.playerAvatar.GetComponent<PlayerWeaponsManager>();
        m_weaponManager.onAddedWeapon += onAddWeaponHandler;
        m_inventory = localplayer.playerAvatar.GetComponent<Inventory>();
    }

    public void OnDiedHandler()
    {
        m_weaponManager = null;
        m_inventory = null;
    }
}
