using System.Collections;
using Photon.Pun;
using UnityEngine;

public class NetPlayerWeaponController : PlayerWeaponController
{
    PhotonView PV;

    public override void Awake()
    {
        base.Awake();
        PV = GetComponent<PhotonView>();

    }

    public override void UpdateAmmo()
    {
        if (PV.IsMine)
        {
            base.UpdateAmmo();
        }

    }
}
