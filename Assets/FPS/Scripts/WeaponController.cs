using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WeaponShootType
{
    Manual,
    Automatic
}

[System.Serializable]
public struct CrosshairData
{
    [Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite crosshairSprite;
    [Tooltip("The size of the crosshair image")]
    public int crosshairSize;
    [Tooltip("The color of the crosshair image")]
    public Color crosshairColor;
}

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;
    [Tooltip("The image that will be displayed in the UI for this weapon")]
    public Sprite weaponIcon;
    public int currentLevel = 0;

    [Tooltip("Default data for the crosshair")]
    public CrosshairData crosshairDataDefault;
    [Tooltip("Data for the crosshair when targeting an enemy")]
    public CrosshairData crosshairDataTargetInSight;

    [Header("Internal References")]
    [Tooltip("The root object for the weapon, this is what will be deactivated when the weapon isn't active")]
    public GameObject weaponRoot;
    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform weaponMuzzle;

    [Header("Shoot Parameters")]
    [Tooltip("The type of weapon wil affect how it shoots")]
    public WeaponShootType shootType;
    [Tooltip("The projectile prefab")]
    public ProjectileBase projectilePrefab;
    [Tooltip("Minimum duration between two shots")]
    public float delayBetweenShots = 0.5f;
    [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
    public float bulletSpreadAngle = 0f;
    [Tooltip("Amount of bullets per shot")]
    public int bulletsPerShot = 1;
    [Tooltip("Ratio of the default FOV that this weapon applies while aiming")]
    [Range(0f, 1f)]
    public float aimZoomRatio = 1f;
    [Tooltip("Translation to apply to weapon arm when aiming with this weapon")]
    public Vector3 aimOffset;

    [Header("Ammo Parameters")]
    [Tooltip("Amount of ammo reloaded per second")]
    public float ammoReloadRate = 1f;
    [Tooltip("Delay after the last shot before starting to reload")]
    public float ammoReloadDelay = 2f;
    [Tooltip("Maximum amount of ammo in the gun")]
    public int maxAmmoPerLoad = 8;
    [Tooltip("Maximum amount of ammo can carry")]
    public float maxAmmo = 8;

    [Header("Audio & Visual")]
    [Tooltip("Optional weapon animator for OnShoot animations")]
    public Animator weaponAnimator;
    [Tooltip("Prefab of the muzzle flash")]
    public GameObject muzzleFlashPrefab;
    [Tooltip("Unparent the muzzle flash instance on spawn")]
    public bool unparentMuzzleFlash;
    [Tooltip("sound played when shooting")]
    public AudioClip shootSFX;
    [Tooltip("Sound played when changing to this weapon")]
    public AudioClip changeWeaponSFX;

    public UnityAction onShoot;

    public float m_LastTimeShot = Mathf.NegativeInfinity;
    Vector3 m_LastMuzzlePosition;

    public float m_CurrentAmmo;
    public float m_CurrentAmmoCarried;
    public GameObject owner { get; set; }
    public GameObject sourcePrefab { get; set; }
    public float currentAmmoRatio { get; private set; }
    public bool isWeaponActive { get; private set; }
    public bool isCooling { get; private set; }
    public Vector3 muzzleWorldVelocity { get; private set; }

    private const int V = 1;

    public float GetAmmoNeededToShoot() => V / maxAmmo;

    AudioSource m_ShootAudioSource;

    const string k_AnimAttackParameter = "Attack";

    Queue<ProjectileBase> bulletPool;

    public virtual void Awake()
    {
        m_CurrentAmmo = maxAmmoPerLoad;
        m_CurrentAmmoCarried = maxAmmo;

        m_LastMuzzlePosition = weaponMuzzle.position;

        m_ShootAudioSource = GetComponent<AudioSource>();
        DebugUtility.HandleErrorIfNullGetComponent<AudioSource, WeaponController>(m_ShootAudioSource, this, gameObject);

    }
    public virtual void Start()
    {
        bulletPool = new Queue<ProjectileBase>();
        for(int i = 0; i < maxAmmoPerLoad; i++)
        {
            ProjectileBase bullet = Instantiate(projectilePrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

    }

    public void OnDestroy()
    {
        foreach(ProjectileBase bullet in bulletPool)
        {
            Destroy(bullet.gameObject);
        }
    }
    void Update()
    {
        UpdateAmmo();

        if (Time.deltaTime > 0)
        {
            muzzleWorldVelocity = (weaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = weaponMuzzle.position;
        }
    }

    public virtual void UpdateAmmo()
    {
        if (m_LastTimeShot + ammoReloadDelay < Time.time && m_CurrentAmmo < maxAmmo)
        {
            // reloads weapon over time
            m_CurrentAmmo += ammoReloadRate * Time.deltaTime;

            // limits ammo to max value
            m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo, 0, maxAmmo);

            isCooling = true;
        }
        else
        {
            isCooling = false;
        }

        if (maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1f;
        }
        else
        {
            currentAmmoRatio = m_CurrentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);

        if (show && changeWeaponSFX)
        {
            m_ShootAudioSource.PlayOneShot(changeWeaponSFX);
        }

        isWeaponActive = show;
    }

    public void UseAmmo(float amount)
    {
        m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo - amount, 0f, maxAmmo);
        m_LastTimeShot = Time.time;
    }

    public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Manual:
                if (inputDown)
                {
                    return TryShoot();
                }
                return false;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    return TryShoot();
                }
                return false;

            default:
                return false;
        }
    }

    bool TryShoot()
    {
        if (m_CurrentAmmo >= 1f 
            && m_LastTimeShot + delayBetweenShots < Time.time)
        {
            HandleShoot();
            m_CurrentAmmo -= 1;

            return true;
        }

        return false;
    }

    void HandleShoot()
    {
        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
            ProjectileBase newProjectile = spawnBullet(weaponMuzzle.position, Quaternion.LookRotation(shotDirection));
            newProjectile.Shoot(this);
        }

        // muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle.transform);
            // Unparent the muzzleFlashInstance
            if (unparentMuzzleFlash)
            {
                muzzleFlashInstance.transform.SetParent(null);
            }

            Destroy(muzzleFlashInstance, 2f);
        }

        // play shoot SFX
        if (shootSFX)
        {
            m_ShootAudioSource.PlayOneShot(shootSFX);
        }

        m_LastTimeShot = Time.time;

        // Trigger attack animation if there is any
        if (weaponAnimator)
        {
            weaponAnimator.SetTrigger(k_AnimAttackParameter);
        }

        // Callback on shoot
        if (onShoot != null)
        {
            onShoot();
        }
    }

    public virtual Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection;

        spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);

        return spreadWorldDirection;
    }


    public ProjectileBase spawnBullet(Vector3 location, Quaternion rotation) {
        ProjectileBase bullet = bulletPool.Dequeue();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = location;
        bullet.transform.rotation = rotation;

        bulletPool.Enqueue(bullet);

        return bullet;

    }

}
