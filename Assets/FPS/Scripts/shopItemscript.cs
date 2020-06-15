using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New shop item")]
public class shopItemscript : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public Sprite itemImage;
}
