using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIntToOpen : MonoBehaviour
{
    public GameObject textline;
    // Start is called before the first frame update
    void Start()
    {
        textline.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StationTriggerManager.OnStayedStation += showHintText;
        StationTriggerManager.OnExitedStation += HideHintText;
    }
    private void OnDisable()
    {
        StationTriggerManager.OnStayedStation -= showHintText;
        StationTriggerManager.OnExitedStation -= HideHintText;
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
