using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scorekeeper : MonoBehaviour
{
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
        GUI.Box(new Rect(100, 100, 100, 70), "Score");

    }
}
