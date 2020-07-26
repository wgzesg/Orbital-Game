using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public TMPro.TextMeshProUGUI currentScore;
    public TMPro.TextMeshProUGUI highScore;
    // Start is called before the first frame update
    void Start()
    {
        currentScore.text = "Score: " + PlayerPrefs.GetInt("CurrentScore").ToString();
        highScore.text = "Highest score: " + PlayerPrefs.GetInt("HighestScore").ToString();
    }
}
