using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HintToOpen : MonoBehaviour
{
    public GameObject canvasRoot;
    public Slider ReloadingBar;
    public StationTriggerManager station;
    public PlayerWeaponsManager m_weaponManager;
    public Coroutine refilling;
    // Start is called before the first frame update
    public virtual void Start()
    {
        station = GetComponent<StationTriggerManager>();
        m_weaponManager = FindObjectOfType<PlayerWeaponsManager>();
        canvasRoot.SetActive(false);
        station.OnEnteredStation += OnEnterHandler;
        station.OnExitedStation += OnExitHandler;
    }

    public void OnEnterHandler()
    {
        canvasRoot.SetActive(true);
        bool isAllFull = true;
        foreach (WeaponController thisWeapon in m_weaponManager.m_WeaponSlots)
        {
            if (thisWeapon != null && thisWeapon.m_CurrentAmmoCarried != thisWeapon.maxAmmo)
            {
                isAllFull = false;
                break;
            }
        }
        if (isAllFull == false)
            refilling = StartCoroutine(RefillCoroutine());
    }

    public void OnExitHandler()
    {
        canvasRoot.SetActive(false);
        if(refilling != null)
            StopCoroutine(refilling);
    }

    public IEnumerator RefillCoroutine()
    {
        ReloadingBar.gameObject.SetActive(true);
        float t = 0f;
        while (t < 5)
        {
            Debug.Log("update refill bar");
            yield return new WaitForSeconds(0.1f);
            t = t + 0.1f;
            ReloadingBar.value = t * 2 / 9;
        }
        ReloadingBar.gameObject.SetActive(false);
        foreach (WeaponController thisWeapon in m_weaponManager.m_WeaponSlots)
        {
            if(thisWeapon != null)
                thisWeapon.m_CurrentAmmoCarried = thisWeapon.maxAmmo;
        }

    }
}
