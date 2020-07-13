using UnityEngine;
using UnityEngine.Events;

public class GearScript : MonoBehaviour
{
    UnityAction onPickUp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onPickUp += other.GetComponent<Inventory>().onPickUp;
            Destroy(transform.parent.gameObject);
            onPickUp.Invoke();
        }
    }
}
