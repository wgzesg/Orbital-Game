using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    public shopItemscript thePurchasedItem;
    public WeaponPickup gunDropPrefab;
    public Transform dropPoint;

    Inventory m_inventory;

    PlayerWeaponsManager m_weaponManager;

    private void Start()
    {
        m_weaponManager = FindObjectOfType<PlayerWeaponsManager>();
        m_inventory = FindObjectOfType<Inventory>();

    }


    public void OnButtonClicked(shopItemscript item)
    {
        if(item.itemPrice > m_inventory.gearCount)
        {
            Debug.Log("Not enough gear to purchase the item.");
            return;
        }
        Debug.Log("The purchased one is " + item.itemName);
        bool hasWeapon = false;
        foreach(PlayerWeaponController weapon in m_weaponManager.startingWeapons)
        {
            if (weapon.weaponName == item.itemName)
            {
                hasWeapon = true;
                break;
            }
        }

        if(hasWeapon == true)
        {
            Debug.Log("The player has " + item.itemName);
        }
        else
        {
            GameObject.Instantiate(gunDropPrefab, dropPoint);
        }

    }

}
