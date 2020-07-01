using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public shopItemscript shopitem;
    public new TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI price;
    public Image itemSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (shopitem.itemName == "Pistol")
        {
            shopitem.level = 1;
        }
        else
            shopitem.level = 0;
        name.text = shopitem.itemName;
        price.text = shopitem.itemPrice[shopitem.level].ToString();
        itemSprite.sprite = shopitem.itemImage;
    }

    public void onUpdatePrice()
    {
        price.text = shopitem.itemPrice[shopitem.level].ToString();
    }

}
