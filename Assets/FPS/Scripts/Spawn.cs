using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;


    private ObjectiveKillEnemies killObjective;
    private int numOfwaves = 0;
    private int targetWaves;
    EnemyManager m_enemyManager;
    StorytellingManager m_story;
    void Start()
    {
        m_enemyManager = GetComponent<EnemyManager>();
        m_enemyManager.onRemoveEnemy += removeEnemyHandler;

        m_story = GetComponent<StorytellingManager>();
        m_story.onStartGame += onStartGameSpawn;

        killObjective = FindObjectOfType<ObjectiveKillEnemies>();
        targetWaves = killObjective.numberOfWaves;

    }

    private void onStartGameSpawn()
    {
        for(int i = 0; i< 10; i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
            Instantiate(enemy, spawnPoints[0].position + randomVector, Quaternion.identity);
        }
        numOfwaves++;
    }

    private void removeEnemyHandler(EnemyController diedOne, int enemyleft)
    {
        if(enemyleft <= 3 && numOfwaves < targetWaves)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            for(int i = 0; i < 10; i++)
            {
                Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
                Instantiate(enemy, spawnPoint.position + randomVector, Quaternion.identity);
            }
            numOfwaves++;
        }
    }


}
