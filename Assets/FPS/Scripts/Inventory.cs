using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int gearCount;
    public UnityAction<int> onUpdateGearCount;

    public virtual void Start()
    {
        gearCount = 0;
    }

    public void onPickUp()
    {
        Debug.Log("I picked up item");
        gearCount ++;
        if (onUpdateGearCount != null)
            onUpdateGearCount.Invoke(gearCount);
    }
}
