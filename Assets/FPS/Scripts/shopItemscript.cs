using UnityEngine;

[CreateAssetMenu(fileName ="New shop item")]
public class shopItemscript : ScriptableObject
{
    public string itemName;
    public int level;
    public int[] itemPrice;
    public Sprite itemImage;

}
