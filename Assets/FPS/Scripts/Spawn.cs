using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform spawnPoint;

    EnemyManager m_enemyManager;
    void Start()
    {
        m_enemyManager = FindObjectOfType<EnemyManager>();
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
            Instantiate(enemy, spawnPoint.position + randomVector, Quaternion.identity);
        }
    }

    private void Update()
    {
        if(m_enemyManager.enemies.Count <= 3)
        {
            for(int i = 0; i < 10; i++)
            {
                Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
                Instantiate(enemy, spawnPoint.position + randomVector, Quaternion.identity);
            }
        }
    }


}
