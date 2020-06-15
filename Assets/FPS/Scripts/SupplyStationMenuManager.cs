using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SupplyStationMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;
    
    [Tooltip("GameObject for the controls")]
    public GameObject controlImage;

    PlayerInputHandler m_PlayerInputsHandler;

    void Start()
    {
        m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);


        menuRoot.SetActive(false);
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown("Interaction")
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

    public void OnButtonClicked(shopItemscript item)
    {
        Debug.Log("The one selected is " + item.itemName);
    }




    public void OnShowControlButtonClicked(bool show)
    {
        controlImage.SetActive(show);
    }
}
