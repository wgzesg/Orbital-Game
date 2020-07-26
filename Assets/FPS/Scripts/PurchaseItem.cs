using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseItem : MonoBehaviour
{
    public shopItemscript thePurchasedItem;
    public WeaponPickup gunDropPrefab;
    public Transform dropPoint;

    public TMPro.TextMeshProUGUI countDown;
    public Button purchaseKey;

    protected WeaponController existingWeapon;
    protected Inventory m_inventory;
    protected PlayerWeaponsManager m_weaponManager;
    protected ItemDisplay m_itemDisplay;
    protected TMPro.TextMeshProUGUI buttonText;

    public virtual void Start()
    {
        m_weaponManager = FindObjectOfType<PlayerWeaponsManager>();
        m_weaponManager.onAddedWeapon += onAddWeaponHandler;
        m_inventory = FindObjectOfType<Inventory>();
        m_itemDisplay = GetComponent<ItemDisplay>();
        buttonText = purchaseKey.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        countDown.gameObject.SetActive(false);

    }


    public void OnButtonClicked(shopItemscript item)
    {
        if (item.itemPrice[item.level] > m_inventory.gearCount)
        {
            Debug.Log("Not enough gear to purchase the item.");
            return;
        }
        Debug.Log("The purchased one is " + item.itemName);

        if(m_weaponManager.HasWeapon(gunDropPrefab.weaponPrefab))  // If the player has the gun
        {
            Debug.Log("The player has " + item.itemName);

            m_weaponManager.RemoveWeapon(existingWeapon);
            StartCoroutine(Upgrading(item));

            purchaseKey.interactable = false;
            buttonText.text = "Upgrade";
            m_itemDisplay.onUpdatePrice();
        }
        else // If it is first purchase
        {
            WeaponPickup newWeapon = GameObject.Instantiate(gunDropPrefab, dropPoint);
            m_inventory.gearCount -= item.itemPrice[item.level];
            m_inventory.onUpdateGearCount.Invoke(m_inventory.gearCount);

            item.level++;
            purchaseKey.interactable = false;
            buttonText.text = "Upgrade";
            m_itemDisplay.onUpdatePrice();


        }

    }

    protected IEnumerator Upgrading(shopItemscript item)
    {
        purchaseKey.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);

        m_inventory.onUpdateGearCount.Invoke(m_inventory.gearCount);
        int upgradingTime = 5;

        while (upgradingTime > 0)
        {
            countDown.text = upgradingTime.ToString();
            Debug.Log(upgradingTime + " seconds left.");
            yield return new WaitForSecondsRealtime(1f);
            upgradingTime--;
        }

        purchaseKey.gameObject.SetActive(true);
        countDown.gameObject.SetActive(false);


        WeaponPickup newWeapon = GameObject.Instantiate(gunDropPrefab, dropPoint);

        item.level++;
        if(item.level < 4)
        {
            m_itemDisplay.onUpdatePrice();
        }
        else
        {
            purchaseKey.interactable = false;
        }
    }

    public void onAddWeaponHandler(WeaponController addedWeapon, int index)
    {
        if(addedWeapon.weaponName == thePurchasedItem.itemName)
        {
            existingWeapon = addedWeapon;
            purchaseKey.interactable = true;
        }
    }

}
