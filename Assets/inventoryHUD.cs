using Photon.Pun;
using UnityEngine;

public class inventoryHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI inventoryDisplay;

    public Inventory playerInventory;
    // Start is called before the first frame update


    public virtual void Start()
    {
        Inventory[] listOfInvent = FindObjectsOfType<Inventory>();
        if (listOfInvent.Length == 1)
        {
            playerInventory = FindObjectOfType<Inventory>();
        }
        else
        {
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
            {
                Debug.Log(cur);
                Debug.Log("Iterating");
                if (cur.GetComponent<PhotonView>().IsMine == true)
                {
                    Debug.Log("Found local player");
                    playerInventory = cur.GetComponent<Inventory>();
                    break;
                }
            }
        }

        inventoryDisplay.text = "Gears: " + playerInventory.gearCount;

        playerInventory.onUpdateGearCount += OnChangedGearCount;
    }

    public void OnChangedGearCount(int newCount)
    {
        inventoryDisplay.text = "Gears: " + playerInventory.gearCount;
    }
}
