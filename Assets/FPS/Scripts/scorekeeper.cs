using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour
{

    public TMPro.TextMeshProUGUI ScoreboardText;

    private EnemyManager enemyManager;
    int score = 0;

    
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

    void OnRemoveEnemy(EnemyController deadEnemy, int enemyLeft)
    {
        score += 1;

        ScoreboardText.text = "Score: " + score;

        if (GetComponent<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
