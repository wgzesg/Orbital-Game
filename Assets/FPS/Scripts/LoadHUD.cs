using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ammutext;

    float maxLoad;
    float currentLoad;

    PlayerWeaponsManager m_playerWeaponsManager;
    WeaponController curr_activeWeapon;

    private void Start()
    {
        m_playerWeaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, LoadHUD>(m_playerWeaponsManager, this);

        WeaponController m = m_playerWeaponsManager.GetActiveWeapon();
        curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();

        m_playerWeaponsManager.onSwitchedToWeapon += OnswitchWeaponHandler;
    }

    private void OnswitchWeaponHandler(WeaponController newpweaon)
    {
        curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();
    }

    void Update()
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

