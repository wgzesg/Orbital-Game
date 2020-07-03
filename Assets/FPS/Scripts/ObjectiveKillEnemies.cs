using UnityEngine;

[RequireComponent(typeof(Objective))]
public class ObjectiveKillEnemies : MonoBehaviour
{
    [Tooltip("Start sending notification about remaining enemies when this amount of enemies is left")]
    public int notificationEnemiesRemainingThreshold = 3;
    public int notificationWavesRemainingThreshold = 3;
    public int targetWaves = 1;

    private int defeatedWaves;
    EnemyManager m_EnemyManager;
    Objective m_Objective;
    Spawn m_spawnManager;
    int m_KillTotal;

    void Start()
    {
        m_Objective = GetComponent<Objective>();
        DebugUtility.HandleErrorIfNullGetComponent<Objective, ObjectiveKillEnemies>(m_Objective, this, gameObject);

        m_EnemyManager = FindObjectOfType<EnemyManager>();
        DebugUtility.HandleErrorIfNullFindObject<EnemyManager, ObjectiveKillEnemies>(m_EnemyManager, this);
        m_EnemyManager.onRemoveEnemy += OnKillEnemy;

        m_spawnManager = FindObjectOfType<Spawn>();
        DebugUtility.HandleErrorIfNullFindObject<Spawn, ObjectiveKillEnemies>(m_spawnManager, this);
        m_spawnManager.onSpawn += OnSpawnHandler;
        

        // set a title and description specific for this type of objective, if it hasn't one
        if (string.IsNullOrEmpty(m_Objective.title))
            m_Objective.title = "Eliminate all the enemies";

        if (string.IsNullOrEmpty(m_Objective.description))
            m_Objective.description = GetUpdatedCounterAmount();
    }

    void OnKillEnemy(EnemyController enemy, int remaining)
    {
        m_KillTotal = m_EnemyManager.numberOfEnemiesTotal - remaining;
        int targetRemaning = remaining;

        // create a notification text if needed, if it stays empty, the notification will not be created
        string notificationText = notificationEnemiesRemainingThreshold >= targetRemaning ? targetRemaning + " enemies to kill left" : string.Empty;
        m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
    }


    void OnSpawnHandler(int wavesDefeated, int totalDeployed)
    {
        m_KillTotal = m_EnemyManager.numberOfEnemiesTotal - totalDeployed;
        int targetRemaning = totalDeployed;
        int remainingWaves = targetWaves - wavesDefeated;
        defeatedWaves = wavesDefeated;
        if (defeatedWaves == targetWaves)
        {
            m_Objective.CompleteObjective(string.Empty, GetUpdatedCounterAmount(), "Objective complete : " + m_Objective.title);
        }
        else
        {
            string notificationText = notificationWavesRemainingThreshold >= remainingWaves ? remainingWaves + " waves of enemeis to kill left" : string.Empty;
            m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
        }

    }

    string GetUpdatedCounterAmount()
    {
        return defeatedWaves + " / " + targetWaves;
    }

}
