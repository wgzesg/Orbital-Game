using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ammutext;

    public float maxLoad;
    public float currentLoad;

    public PlayerWeaponsManager m_playerWeaponsManager;
    public WeaponController curr_activeWeapon;

    public virtual void Start()
    {
        m_playerWeaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, LoadHUD>(m_playerWeaponsManager, this);

        curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();

        m_playerWeaponsManager.onSwitchedToWeapon += OnswitchWeaponHandler;
    }

    public void OnswitchWeaponHandler(WeaponController newpweaon)
    {
        curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();
    }

    public virtual void Update()
    {

        maxLoad = curr_activeWeapon.m_CurrentAmmoCarried;
        currentLoad = curr_activeWeapon.m_CurrentAmmo;
        ammutext.text = currentLoad + " / " + maxLoad;
        if (GetComponent<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

    }
}

