using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HIntToOpen : MonoBehaviour
{
    public GameObject canvasRoot;
    public Slider ReloadingBar;
    private StationTriggerManager station;
    private PlayerWeaponsManager m_weaponManager;
    private Coroutine refilling;
    // Start is called before the first frame update
    void Start()
    {
        station = GetComponent<StationTriggerManager>();
        m_weaponManager = FindObjectOfType<PlayerWeaponsManager>();
        canvasRoot.SetActive(false);
        station.OnEnteredStation += OnEnterHandler;
        station.OnExitedStation += OnExitHandler;
    }

    private void OnEnterHandler()
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

    private void OnExitHandler()
    {
        canvasRoot.SetActive(false);
        if(refilling != null)
            StopCoroutine(refilling);
    }

    private IEnumerator RefillCoroutine()
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
