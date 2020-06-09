using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scorekeeper : MonoBehaviour
{
    public int x_cood = 50;
    public int y_cood= 50;
    public int length = 100;
    public int width = 75;

    private EnemyManager enemyManager;
    public int score = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        score = enemyManager.numberOfDeaths;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(x_cood, y_cood, length, width), "Score: "+score);

    }
}
