using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerWeaponsManager : MonoBehaviour
{
    public enum WeaponSwitchState
    {
        Up,
        Down,
        PutDownPrevious,
        PutUpNew,
        Reloading
    }

    [Tooltip("List of weapon the player will start with")]
    public List<PlayerWeaponController> startingWeapons = new List<PlayerWeaponController>();

    [Header("References")]
    [Tooltip("Secondary camera used to avoid seeing weapon go throw geometries")]
    public Camera weaponCamera;
    [Tooltip("Parent transform where all weapon will be added in the hierarchy")]
    public Transform weaponParentSocket;
    [Tooltip("Position for weapons when active but not actively aiming")]
    public Transform defaultWeaponPosition;
    [Tooltip("Position for innactive weapons")]
    public Transform downWeaponPosition;
    [Tooltip("Correction for pistol")]
    public Vector3 pistolOffset;
    public Vector3 pistolRotation;

    [Header("Misc")]
    [Tooltip("Field of view when not aiming")]
    public float defaultFOV = 60f;
    [Tooltip("Portion of the regular FOV to apply to the weapon camera")]
    public float weaponFOVMultiplier = 1f;
    [Tooltip("Delay before switching weapon a second time, to avoid recieving multiple inputs from mouse wheel")]
    public float weaponSwitchDelay = 1f;
    [Tooltip("Layer to set FPS weapon gameObjects to")]
    public LayerMask FPSWeaponLayer;

    public bool isPointingAtEnemy { get; private set; }
    public int activeWeaponIndex { get; private set; }
    public WeaponSwitchState m_WeaponSwitchState;

    public UnityAction<WeaponController> onSwitchedToWeapon;
    public UnityAction<WeaponController, int> onAddedWeapon;
    public UnityAction<WeaponController, int> onRemovedWeapon;
    public WeaponController[] m_WeaponSlots = new WeaponController[9]; // 9 available weapon slots

    PlayerInputHandler m_InputHandler;
    PlayerCharacterController m_PlayerCharacterController;
    Vector3 m_WeaponMainLocalPosition;
    Animator m_Anime;
    float m_TimeStartedWeaponSwitch;
    int m_WeaponSwitchNewWeaponIndex;


    private void Start()
    {
        activeWeaponIndex = -1;
        m_WeaponSwitchState = WeaponSwitchState.Down;

        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, PlayerWeaponsManager>(m_InputHandler, this, gameObject);

        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerWeaponsManager>(m_PlayerCharacterController, this, gameObject);

        m_Anime = GetComponent<Animator>();
        DebugUtility.HandleErrorIfNullGetComponent<Actor, PlayerCharacterController>(m_Anime, this, gameObject);

        m_Anime.SetBool("isShooting", false);

        SetFOV(defaultFOV);

        onSwitchedToWeapon += OnWeaponSwitched;

        // Add starting weapons
        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    private void Update()
    {
        // shoot handling
        WeaponController activeWeapon = GetActiveWeapon();

        if (activeWeapon && m_WeaponSwitchState == WeaponSwitchState.Up)
        {

            // handle shooting
            bool hasFired = activeWeapon.HandleShootInputs(
                m_InputHandler.GetFireInputDown(),
                m_InputHandler.GetFireInputHeld(),
                m_InputHandler.GetFireInputReleased());
            
        }

        // weapon switch handling
        if ( activeWeapon == null &&
            (m_WeaponSwitchState == WeaponSwitchState.Up || m_WeaponSwitchState == WeaponSwitchState.Down))
        {
            Debug.Log("entering switch weapon function");
            int switchWeaponInput = m_InputHandler.GetSwitchWeaponInput();
            if (switchWeaponInput != 0)
            {
                Debug.Log("there is a switching event");
                bool switchUp = switchWeaponInput > 0;
                SwitchWeapon(switchUp);
            }
            else
            {
                switchWeaponInput = m_InputHandler.GetSelectWeaponInput();
                if (switchWeaponInput != 0)
                {
                    Debug.Log("there is a switching event");
                    if (GetWeaponAtSlotIndex(switchWeaponInput - 1) != null)
                        SwitchToWeaponIndex(switchWeaponInput - 1);
                }
            }
        }

        // Pointing at enemy handling
        isPointingAtEnemy = false;
        if (activeWeapon)
        {
            if(Physics.Raycast(weaponCamera.transform.position, weaponCamera.transform.forward, out RaycastHit hit, 1000, -1, QueryTriggerInteraction.Ignore))
            {
                if(hit.collider.GetComponentInParent<EnemyController>())
                {
                    isPointingAtEnemy = true;
                }
            }
        }
    }


    // Update various animated features in LateUpdate because it needs to override the animated arm position
    private void LateUpdate()
    {
        UpdateWeaponSwitching();

        UpdateAnimation();

        // Set final weapon socket position based on all the combined animation influences
        weaponParentSocket.localPosition = m_WeaponMainLocalPosition;
    }

    // Sets the FOV of the main camera and the weapon camera simultaneously
    public void SetFOV(float fov)
    {
        m_PlayerCharacterController.playerCamera.fieldOfView = fov;
        weaponCamera.fieldOfView = fov * weaponFOVMultiplier;
    }

    // Iterate on all weapon slots to find the next valid weapon to switch to
    public void SwitchWeapon(bool ascendingOrder)
    {
        int newWeaponIndex = -1;
        int closestSlotDistance = m_WeaponSlots.Length;
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // If the weapon at this slot is valid, calculate its "distance" from the active slot index (either in ascending or descending order)
            // and select it if it's the closest distance yet
            if (i != activeWeaponIndex && GetWeaponAtSlotIndex(i) != null)
            {
                int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(activeWeaponIndex, i, ascendingOrder);

                if (distanceToActiveIndex < closestSlotDistance)
                {
                    closestSlotDistance = distanceToActiveIndex;
                    newWeaponIndex = i;
                }
            }
        }

        // Handle switching to the new weapon index
        SwitchToWeaponIndex(newWeaponIndex);
    }

    // Switches to the given weapon index in weapon slots if the new index is a valid weapon that is different from our current one
    public void SwitchToWeaponIndex(int newWeaponIndex, bool force = false)
    {
        if (force || (newWeaponIndex != activeWeaponIndex && newWeaponIndex >= 0))
        {
            // Store data related to weapon switching animation
            m_WeaponSwitchNewWeaponIndex = newWeaponIndex;
            m_TimeStartedWeaponSwitch = Time.time;

            // Handle case of switching to a valid weapon for the first time (simply put it up without putting anything down first)
            if(GetActiveWeapon() == null)
            {
                m_WeaponMainLocalPosition = downWeaponPosition.localPosition;
                m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                activeWeaponIndex = m_WeaponSwitchNewWeaponIndex;

                WeaponController newWeapon = GetWeaponAtSlotIndex(m_WeaponSwitchNewWeaponIndex);
                if (onSwitchedToWeapon != null)
                {
                    onSwitchedToWeapon.Invoke(newWeapon);
                }
            }
            // otherwise, remember we are putting down our current weapon for switching to the next one
            else
            {
                m_WeaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }
    }

    public bool HasWeapon(WeaponController weaponPrefab)
    {
        // Checks if we already have a weapon coming from the specified prefab
        foreach(var w in m_WeaponSlots)
        {
            if(w != null && w.sourcePrefab == weaponPrefab.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    void UpdateAnimation()
    {
        m_Anime.SetLayerWeight(2, 0);
        m_Anime.SetLayerWeight(1, 0);
        m_Anime.SetLayerWeight(3, 0);
        if (m_WeaponSwitchState == WeaponSwitchState.Reloading)
        {
            m_Anime.SetLayerWeight(3, 1);
        }

        else if (GetActiveWeapon().weaponName == "Pistol")
        {
            m_Anime.SetLayerWeight(2, 1);
        }
        else
        {
            m_Anime.SetLayerWeight(1, 1);
        }
    }

    // Updates the animated transition of switching weapons
    void UpdateWeaponSwitching()
    {
        // Calculate the time ratio (0 to 1) since weapon switch was triggered
        float switchingTimeFactor = 0f;
        if (weaponSwitchDelay == 0f)
        {
            switchingTimeFactor = 1f;
        }
        else
        {
            switchingTimeFactor = Mathf.Clamp01((Time.time - m_TimeStartedWeaponSwitch) / weaponSwitchDelay);
        }

        // Handle transiting to new switch state
        if(switchingTimeFactor >= 1f)
        {
            if (m_WeaponSwitchState == WeaponSwitchState.PutDownPrevious)
            {
                // Deactivate old weapon
                WeaponController oldWeapon = GetWeaponAtSlotIndex(activeWeaponIndex);
                if (oldWeapon != null)
                {
                    oldWeapon.ShowWeapon(false);
                }

                activeWeaponIndex = m_WeaponSwitchNewWeaponIndex;
                switchingTimeFactor = 0f;

                // Activate new weapon
                WeaponController newWeapon = GetWeaponAtSlotIndex(activeWeaponIndex);
                if (onSwitchedToWeapon != null)
                {
                    onSwitchedToWeapon.Invoke(newWeapon);
                }

                if(newWeapon)
                {
                    m_TimeStartedWeaponSwitch = Time.time;
                    m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                }
                else
                {
                    // if new weapon is null, don't follow through with putting weapon back up
                    m_WeaponSwitchState = WeaponSwitchState.Down;
                }
            }
            else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
            {
                m_WeaponSwitchState = WeaponSwitchState.Up;
            }
        }

        // Handle moving the weapon socket position for the animated weapon switching
        if (m_WeaponSwitchState == WeaponSwitchState.PutDownPrevious)
        {
            m_WeaponMainLocalPosition = Vector3.Lerp(defaultWeaponPosition.localPosition, downWeaponPosition.localPosition, switchingTimeFactor);
        }
        else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
        {
            m_WeaponMainLocalPosition = Vector3.Lerp(downWeaponPosition.localPosition, defaultWeaponPosition.localPosition, switchingTimeFactor);
        }
    }

    // Adds a weapon to our inventory
    public bool AddWeapon(WeaponController weaponPrefab)
    {
        // if we already hold this weapon type (a weapon coming from the same source prefab), don't add the weapon
        if(HasWeapon(weaponPrefab))
        {
            return false;
        }

        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if(m_WeaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                weaponInstance.transform.localPosition = (i == 2) ? pistolOffset : Vector3.zero;
                weaponInstance.transform.localRotation = (i == 2) ? Quaternion.Euler(pistolRotation) : Quaternion.identity;

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly
                weaponInstance.owner = gameObject;
                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);
                weaponInstance.m_CurrentAmmo = weaponInstance.maxAmmoPerLoad;

                // Assign the first person layer to the weapon
                int layerIndex = Mathf.RoundToInt(Mathf.Log(FPSWeaponLayer.value, 2)); // This function converts a layermask to a layer index
                foreach (Transform t in weaponInstance.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.layer = layerIndex;
                }

                m_WeaponSlots[i] = weaponInstance;
                
                if(onAddedWeapon != null)
                {
                    onAddedWeapon.Invoke(weaponInstance, i);
                }

                return true;
            }
        }

        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveWeapon() == null)
        {
            SwitchWeapon(true);
        }

        return false;
    }

    public bool RemoveWeapon(WeaponController weaponInstance)
    {
        // Look through our slots for that weapon
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // when weapon found, remove it
            Debug.Log(weaponInstance.weaponName + " is tyring to be removed.");
            if (m_WeaponSlots[i] == weaponInstance)
            {
                m_WeaponSlots[i] = null;

                if (onRemovedWeapon != null)
                {
                    onRemovedWeapon.Invoke(weaponInstance, i);
                }
                

                Destroy(weaponInstance.gameObject);

                // Handle case of removing active weapon (switch to next weapon)
                if(i == activeWeaponIndex)
                {
                    SwitchWeapon(true);
                }

                return true; 
            }
        }

        return false;
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if(index >= 0 &&
            index < m_WeaponSlots.Length)
        {
            return m_WeaponSlots[index];
        }

        // if we didn't find a valid active weapon in our weapon slots, return null
        return null;
    }

    // Calculates the "distance" between two weapon slot indexes
    // For example: if we had 5 weapon slots, the distance between slots #2 and #4 would be 2 in ascending order, and 3 in descending order
    int GetDistanceBetweenWeaponSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
    {
        int distanceBetweenSlots = 0;

        if (ascendingOrder)
        {
            distanceBetweenSlots = toSlotIndex - fromSlotIndex;
        }
        else
        {
            distanceBetweenSlots = -1 * (toSlotIndex - fromSlotIndex);
        }

        if (distanceBetweenSlots < 0)
        {
            distanceBetweenSlots = m_WeaponSlots.Length + distanceBetweenSlots;
        }

        return distanceBetweenSlots;
    }

    void OnWeaponSwitched(WeaponController newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.ShowWeapon(true);
        }
    }

}
