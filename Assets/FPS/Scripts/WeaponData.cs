using UnityEngine;

[CreateAssetMenu(fileName = "New weapon data")]
public class WeaponData : ScriptableObject
{
    [System.Serializable]
    public struct Level{
        public int level;
        public GameObject bulletPrefab;
    }
    public Level[] levelsystem;
    public int currentLevel = 0;
}
