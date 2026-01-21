using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _damageDistance = 1.5f;
    [SerializeField] private int _damagePerSecond = 5;
    [SerializeField] private float _spawnInterval = 7f;
    [SerializeField] private float _spawnDistanceFromCamera = 3f;
    [SerializeField] private int _maxEnemiesCount = 20;

    public int MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public float StopDistance => _stopDistance;
    public float DamageDistance => _damageDistance;
    public int DamagePerSecond => _damagePerSecond;
    public float SpawnInterval => _spawnInterval;
    public float SpawnDistanceFromCamera => _spawnDistanceFromCamera;
    public int MaxEnemiesCount => _maxEnemiesCount;
}
