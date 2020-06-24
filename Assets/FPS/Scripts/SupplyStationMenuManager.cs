using UnityEngine;
using UnityEngine.EventSystems;

public class SupplyStationMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;

    public TMPro.TextMeshProUGUI gearCountDisplay;

    PlayerInputHandler m_PlayerInputsHandler;
    StationTriggerManager m_stationTriggerManager;
    Inventory m_inventory;
    bool isIn = false;

    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);

        m_stationTriggerManager = FindObjectOfType<StationTriggerManager>();
        m_inventory = FindObjectOfType<Inventory>();
        m_inventory.onUpdateGearCount += onChangeGearCount;

        m_stationTriggerManager.OnEnteredStation += OnEnter;
        m_stationTriggerManager.OnExitedStation += OnExit;
        menuRoot.SetActive(false);
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown("Interaction") && isIn
            || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {

            SetShopMenuActivation(!menuRoot.activeSelf);

        }

        if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public void CloseShopMenu()
    {
        SetShopMenuActivation(false);
    }

    void SetShopMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            gearCountDisplay.text = "Gears: " + m_inventory.gearCount;

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            AudioUtility.SetMasterVolume(1);
        }

    }

    void OnEnter()
    {
        isIn = true;
    }

    void OnExit()
    {
        isIn = false;
    }

    public void onChangeGearCount(int newGearCount)
    {
        gearCountDisplay.text = "Gears: " + newGearCount;
    }

}
