﻿using UnityEngine;
using UnityEngine.UI;

public class scorekeeper : MonoBehaviour
{

    public TMPro.TextMeshProUGUI ScoreboardText;

    private EnemyManager enemyManager;

    int score = 0;
    float startTime = 0.0f;
    float checkPointTime = 0.0f;
    public float criticalDuration = 10.0f;
    public int criticalKilledNum = 5;
    int killedNum = 0;
    int multiplierIndex = 1;
    public int multiplierIndexLimit = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyManager.onRemoveEnemy += OnRemoveEnemy;

        ScoreboardText.text = "Score: " + score;
        if (GetComponent<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    void OnRemoveEnemy(EnemyController deadEnemy, int enemyLeft)   // edit such that the score will multiply
    {
        scoreMultiplier();

        ScoreboardText.text = "Score: " + score;

        if (GetComponent<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    //each increment of score is based on the time difference and number of enemies killed in given time duration
    void scoreMultiplier()
    {
        checkPointTime = Time.time;
        if(checkPointTime - startTime <= criticalDuration)
        {
            Debug.Log("within");
            killedNum++;
            Debug.Log("kill num is " + killedNum);
            if (multiplierIndex < multiplierIndexLimit)
            {
                multiplierIndex++;
                Debug.Log("multiplx is " + multiplierIndex);
            }

            if (killedNum >= criticalKilledNum)
            {
                killedNum = 0;
            }
        }
        else
        {
            Debug.Log("outside");
            killedNum = 1;
            multiplierIndex = 1;
            Debug.Log("multiplx is " + multiplierIndex);
        }
        score += multiplierIndex;
        startTime = Time.time;

    }
}
