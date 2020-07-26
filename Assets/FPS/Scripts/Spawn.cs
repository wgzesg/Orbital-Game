using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.Events;

public class Spawn : MonoBehaviour
{
    public Transform[] spawnPoints;
    public levelData levelDataFile;
    public UnityAction<int, int> onSpawn;

    public ObjectiveKillEnemies killObjective;
    public int numOfwaves = 0;
    public int targetWaves;
    EnemyManager m_enemyManager;
    StorytellingManager m_story;


    public virtual void Start()
    {
        m_enemyManager = GetComponent<EnemyManager>();
        m_enemyManager.onRemoveEnemy += removeEnemyHandler;

        m_story = GetComponent<StorytellingManager>();
        m_story.onStartGame += onStartGameSpawn;

        killObjective = FindObjectOfType<ObjectiveKillEnemies>();
        targetWaves = killObjective.targetWaves;

    }

    public virtual void onStartGameSpawn()
    {
        SpwanNewWave();
    }

    public virtual void removeEnemyHandler(EnemyController diedOne, int enemyleft)
    {
        if (enemyleft == 0)
        {
            SpwanNewWave();
        }
    }

    public virtual void SpwanNewWave()
    {
        List<Transform> spawnPointList = spawnPoints.OrderBy(x => Guid.NewGuid()).Take(levelDataFile.levelsystem[numOfwaves].numberOfSpots).ToList();
        GameObject[] enemyForms = levelDataFile.levelsystem[numOfwaves].enemyForm;
        int numPerPoint = levelDataFile.levelsystem[numOfwaves].numToSpawnAtEachPoint;
        foreach (Transform spawnpoint in spawnPointList)
        {
            for (int i = 0; i < numPerPoint; i++){
                Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
                Instantiate(enemyForms[Random.Range(0, enemyForms.Length)], spawnpoint.position + randomVector, Quaternion.identity);
            }
        }
        int totalDeployed = levelDataFile.levelsystem[numOfwaves].numberOfSpots * levelDataFile.levelsystem[numOfwaves].numToSpawnAtEachPoint;
        if (onSpawn != null)
        {
            onSpawn.Invoke(numOfwaves, totalDeployed);
        }
        numOfwaves++;
    }


}
