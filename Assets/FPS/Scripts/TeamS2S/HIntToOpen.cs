using UnityEngine;

public class HIntToOpen : MonoBehaviour
{
    public GameObject textline;
    private StationTriggerManager station;
    // Start is called before the first frame update
    void Start()
    {
        station = GetComponent<StationTriggerManager>();
        textline.SetActive(false);
        station.OnEnteredStation += showHintText;
        station.OnExitedStation += HideHintText;
    }


    void showHintText()
    {
        textline.SetActive(true);
    }
    void HideHintText()
    {
        textline.SetActive(false);
    }
}
