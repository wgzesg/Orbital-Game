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
        name.text = shopitem.itemName;
        price.text = shopitem.itemPrice.ToString();
        itemSprite.sprite = shopitem.itemImage;
    }

}
