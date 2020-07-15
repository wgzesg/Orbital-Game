using Photon.Pun;
using UnityEngine;

public class inventoryHUD : MonoBehaviour
{
    public TMPro.TextMeshProUGUI inventoryDisplay;

    private Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
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

        playerInventory.onUpdateGearCount += OnChangedGearCount;
    }

    void OnChangedGearCount(int newCount)
    {
        inventoryDisplay.text = "Gears: " + playerInventory.gearCount;
    }
}
