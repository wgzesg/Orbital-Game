using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    public shopItemscript weaponData;
    PlayerInputHandler m_InputHandler;
    GameObject mainCam;
    PlayerWeaponsManager m_weaponManager;
    PhotonView PV;

    public override void Awake()
    {
        base.Awake();
        m_InputHandler = GetComponentInParent<PlayerInputHandler>();

        m_weaponManager = GetComponentInParent<PlayerWeaponsManager>();

        if(GetComponentInParent<PhotonView>() != null)
            PV = GetComponentInParent<PhotonView>();

        if (GetComponentInParent<PlayerCharacterController>() != null)
            mainCam = GameObject.Find("Camera");

    }
    public override void Start()
    {
        currentLevel = weaponData.level - 1;
        if (currentLevel < 0)
            currentLevel = 0;
    }

    public override void UpdateAmmo()
    {
        if (!m_InputHandler.GetReloadInputReleased())
        {
            return;
        }
        if(PV == null ||(PV.IsMine))
            StartCoroutine(loadingCoroutine());

    }

    IEnumerator loadingCoroutine()
    {
        m_weaponManager.m_WeaponSwitchState = PlayerWeaponsManager.WeaponSwitchState.Reloading;

        yield return new WaitForSeconds(3.3f);
        if(m_CurrentAmmoCarried > maxAmmoPerLoad)
        {
            m_CurrentAmmo = maxAmmoPerLoad;
            m_CurrentAmmoCarried -= maxAmmoPerLoad;
        }
        else
        {
            m_CurrentAmmo = m_CurrentAmmoCarried;
            m_CurrentAmmoCarried = 0;
        }

        m_weaponManager.m_WeaponSwitchState = PlayerWeaponsManager.WeaponSwitchState.Up;
    }

    public override Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection;

        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, 1000, -1, QueryTriggerInteraction.Ignore))
        {
            shootTransform.LookAt(hit.transform);
            spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }
        else
        {
            spreadWorldDirection = Vector3.Slerp(mainCam.transform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }

        return spreadWorldDirection;
    }
}
