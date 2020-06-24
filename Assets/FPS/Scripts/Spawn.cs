using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform[] spawnPoints;

    EnemyManager m_enemyManager;
    void Start()
    {
        m_enemyManager = FindObjectOfType<EnemyManager>();
        m_enemyManager.onRemoveEnemy += removeEnemyHandler;
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
            Instantiate(enemy, spawnPoints[0].position + randomVector, Quaternion.identity);
        }
    }

    private void removeEnemyHandler(EnemyController diedOne, int enemyleft)
    {
        if(enemyleft <= 3)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            for(int i = 0; i < 10; i++)
            {
                Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
                Instantiate(enemy, spawnPoint.position + randomVector, Quaternion.identity);
            }
        }
    }


}
