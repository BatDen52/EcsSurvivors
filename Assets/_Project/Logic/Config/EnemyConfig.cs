using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _damageDistance = 1.5f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private int _damageInterval = 1;
    [SerializeField] private float _spawnInterval = 7f;
    [SerializeField] private float _spawnDistanceFromCamera = 3f;
    [SerializeField] private int _maxEnemiesCount = 20;
    [SerializeField] private EntityLink _enemyPrefab;
    [SerializeField] private EnemyHealthBar _healthBarPrefab;

    public int MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public float StopDistance => _stopDistance;
    public float DamageDistance => _damageDistance;
    public int Damage => _damage;
    public int DamageInterval => _damageInterval;
    public float SpawnInterval => _spawnInterval;
    public float SpawnDistanceFromCamera => _spawnDistanceFromCamera;
    public int MaxEnemiesCount => _maxEnemiesCount;
    public EntityLink EnemyPrefab => _enemyPrefab;
    public EnemyHealthBar HealthBarPrefab => _healthBarPrefab; 
}
