using UnityEngine;
using UnityEngine.Events;

public class GearScript : MonoBehaviour
{
    private Inventory playerInventory;
    UnityAction onPickUp;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
        onPickUp += playerInventory.onPickUp;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject);
            onPickUp.Invoke();
        }
    }
}
