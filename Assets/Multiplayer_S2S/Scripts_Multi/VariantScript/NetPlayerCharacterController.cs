using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler), typeof(AudioSource))]
public class NetPlayerCharacterController: PlayerCharacterController
{
    PhotonView PV;
    public override void Start()
    {
        base.Start();
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
        }
    }

    public override void Update()
    {
        if (PV.IsMine)
        {
            base.Update();
        }
    }
}
