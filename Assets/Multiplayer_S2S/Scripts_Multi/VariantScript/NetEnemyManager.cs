using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetEnemyManager : MonoBehaviour
{
    public PlayerCharacterController m_PlayerController;

    public List<EnemyController> enemies { get; private set; }
    public int numberOfEnemiesTotal { get; private set; }
    public int numberOfEnemiesRemaining => enemies.Count;
    
    public UnityAction<EnemyController, int> onRemoveEnemy;
    public int numberOfDeaths = 0;

    private void Awake()
    {
        enemies = new List<EnemyController>();
    }

    private void Start()
    {
        m_PlayerController = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);

        numberOfEnemiesTotal++;
    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesRemaining - 1;

        if (onRemoveEnemy != null)
        {
            onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);
            numberOfDeaths += 1;
        }

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
    }
}
