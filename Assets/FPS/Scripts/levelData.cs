using UnityEngine;

[CreateAssetMenu(fileName = "New gameLevel data")]
public class levelData : ScriptableObject
{
    [System.Serializable]
    public struct gameLevel
    {
        public int level;
        public int numberOfSpots;
        public int numToSpawnAtEachPoint;
        public GameObject[] enemyForm;
    }
    public gameLevel[] levelsystem;
}