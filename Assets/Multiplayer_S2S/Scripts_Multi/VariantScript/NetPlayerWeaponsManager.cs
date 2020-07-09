using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class NetPlayerWeaponsManager : PlayerWeaponsManager
{
    PhotonView PV;

    public override void Start()
    {
        PV = GetComponent<PhotonView>();
        base.Start();

    }
    
    public override void Update()
    {
        if (PV.IsMine)
        {
            base.Update();
        }
    }
}
