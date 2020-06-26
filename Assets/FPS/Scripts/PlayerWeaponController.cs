using System.Collections;
using UnityEngine;

public class PlayerWeaponController : WeaponController
{
    PlayerInputHandler m_InputHandler;
    Animator m_Anime;
    GameObject mainCam;
    PlayerWeaponsManager m_weaponManager;

    public override void Awake()
    {
        base.Awake();
        m_InputHandler = GetComponentInParent<PlayerInputHandler>();

        m_Anime = GetComponentInParent<Animator>();

        m_weaponManager = GetComponentInParent<PlayerWeaponsManager>();

        if (GetComponentInParent<PlayerCharacterController>() != null)
            mainCam = GameObject.Find("Camera");

    }

    public override void UpdateAmmo()
    {
        if (m_InputHandler.GetReloadInputReleased())
        {
            StartCoroutine(loadingCoroutine());
        }

    }

    IEnumerator loadingCoroutine()
    {
        m_weaponManager.m_WeaponSwitchState = PlayerWeaponsManager.WeaponSwitchState.Reloading;

        yield return new WaitForSeconds(3.3f);
        if(base.m_CurrentAmmoCarried > base.maxAmmoPerLoad){
            base.m_CurrentAmmo = base.maxAmmoPerLoad;
            base.m_CurrentAmmoCarried -= base.maxAmmoPerLoad;
        }
        else
        {
            base.m_CurrentAmmo = base.m_CurrentAmmoCarried;
            base.m_CurrentAmmoCarried = 0;
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
