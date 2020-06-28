using UnityEngine;
using UnityEngine.EventSystems;

public class SupplyStationMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public CanvasGroup menuRoot;

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
        menuRoot.alpha = 0;
        menuRoot.interactable = false;
        menuRoot.blocksRaycasts = false;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Interaction") && isIn)
        {

            SetShopMenuActivation(1);

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
        SetShopMenuActivation(0f);
    }

    void SetShopMenuActivation(float alphaValue)
    {
        menuRoot.alpha = alphaValue;
        menuRoot.interactable = alphaValue == 1;
        menuRoot.blocksRaycasts = alphaValue == 1;
        if (menuRoot.alpha == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gearCountDisplay.text = "Gears: " + m_inventory.gearCount;

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
