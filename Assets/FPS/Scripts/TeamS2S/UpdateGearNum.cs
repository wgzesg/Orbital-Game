using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGearNum : MonoBehaviour
{
    public int NumOfGear = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Gear Num : " + NumOfGear);
    }
}
