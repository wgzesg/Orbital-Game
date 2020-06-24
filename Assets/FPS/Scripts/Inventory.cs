using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int gearCount = 0;
    public UnityAction<int> onUpdateGearCount;


    public void onPickUp()
    {
        gearCount ++;
        if (onUpdateGearCount != null)
            onUpdateGearCount.Invoke(gearCount);
    }
}
