using UnityEngine;

public class inventoryHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI inventoryDisplay;

    private Inventory playerInventory;
    // Start is called before the first frame update
    void Awake()
    {
        playerInventory = FindObjectOfType<Inventory>();
        playerInventory.onUpdateGearCount += OnChangedGearCount;
    }

    void OnChangedGearCount(int newCount)
    {
        inventoryDisplay.text = "Gears: " + playerInventory.gearCount;
    }
}
