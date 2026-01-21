using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerConfig", menuName = "Configs/EnemySpawnerConfig")]
public class EnemySpawnerConfig : ScriptableObject
{
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private float _spawnInterval = 7f;
    [SerializeField] private float _spawnDistanceFromCamera = 3f;
    [SerializeField] private int _maxEnemiesCount = 20;

    public EnemyConfig EnemyConfig => _enemyConfig;
    public float SpawnInterval => _spawnInterval;
    public float SpawnDistanceFromCamera => _spawnDistanceFromCamera;
    public int MaxEnemiesCount => _maxEnemiesCount;
}
