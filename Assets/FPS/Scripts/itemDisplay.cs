using UnityEngine;
using UnityEngine.UI;

public class itemDisplay : MonoBehaviour
{
    public shopItemscriptableItem shopitem;

    public Image itemSprite;

    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI itemPrice;

    public Button purchaseKey;
    void Start()
    {
        itemSprite.sprite = shopitem.itemimage;
        itemName.text = shopitem.itemname;
        itemPrice.text = shopitem.itemmprice.ToString();

    }

    public void OnPurchased()
    {
        Debug.Log(shopitem.itemname + " is purchased.");
    }

    public void OnMouseEnter()
    {
        Debug.Log("The mouse enters " + itemName.text); 
    }

}