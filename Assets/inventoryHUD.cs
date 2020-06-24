using UnityEngine;

public class inventoryHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI inventoryDisplay;

    private Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        inventoryDisplay.text = "Gears: " + 0;
        playerInventory = FindObjectOfType<Inventory>();
        playerInventory.onUpdateGearCount += OnChangedGearCount;
    }

    void OnChangedGearCount(int newCount)
    {
        inventoryDisplay.text = "Gears: " + newCount;
    }
}
