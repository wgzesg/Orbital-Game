using UnityEngine;
using UnityEngine.EventSystems;

public class NetSupplyStationMenuManager: SupplyStationMenuManager
{
    PlayerAvatar localPlayer;
    public override void Start()
    {
        base.Start();
        localPlayer = PlayerManager.PMinstance.findLocalPlayerAvatar();

        localPlayer.PlayerDied += OnDiedHandler;
        localPlayer.PlayerSpawned += OnSpawnedHandler;
        OnSpawnedHandler();
    }

    public void OnDiedHandler()
    {
        m_inventory = localPlayer.playerAvatar.GetComponent<Inventory>();
        m_inventory.onUpdateGearCount += onChangeGearCount;
    }

    public void OnSpawnedHandler()
    {
        m_inventory = null;
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
