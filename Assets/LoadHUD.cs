using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadHUD : MonoBehaviour
{
    public int x_cood = 50;
    public int y_cood = 300;
    public int length = 100;
    public int width = 75;

    float maxLoad;
    float currentLoad;

    PlayerWeaponsManager m_playerWeaponsManager;
    

    private void Start()
    {
        m_playerWeaponsManager = GameObject.FindObjectOfType<PlayerWeaponsManager>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerWeaponsManager, LoadHUD>(m_playerWeaponsManager, this);
    }

    private void OnGUI()
    {
        WeaponController curr_activeWeapon = m_playerWeaponsManager.GetActiveWeapon();

        maxLoad = curr_activeWeapon.maxAmmo;
        currentLoad = curr_activeWeapon.m_CurrentAmmo;
        string content = currentLoad + " / " + maxLoad;
        GUI.Box(new Rect(x_cood, y_cood, length, width), content);

    }
}

