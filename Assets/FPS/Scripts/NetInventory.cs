using UnityEngine;
using UnityEngine.Events;

public class NetInventory : Inventory
{
    PlayerAvatar localPlayer;
    public override void Start()
    {
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();

        gearCount = localPlayer.gearCount;
        Debug.Log("Setting to " + gearCount);
        if (onUpdateGearCount != null)
            onUpdateGearCount.Invoke(gearCount);
    }
}
