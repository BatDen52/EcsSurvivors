using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    [SerializeField] private int _playerMaxHealth = 100;
    [SerializeField] private float _playerMoveSpeed = 5f;
    [SerializeField] private float _shootCooldown = 1f;

    [Header("Projectiles")]
    [SerializeField] private float _projectileSpeed = 12f;
    [SerializeField] private int _projectileDamage = 25;

    [Header("Enemies")]
    [SerializeField] private int _enemyMaxHealth = 100;
    [SerializeField] private float _enemyMoveSpeed = 3f;
    [SerializeField] private float _enemyStopDistance = 1.5f;
    [SerializeField] private float _enemyDamageDistance = 1.5f;
    [SerializeField] private int _enemyDamagePerSecond = 5;
    [SerializeField] private float _enemySpawnInterval = 7f;

    [Header("Coins")]
    [SerializeField] private float _coinDropChance = 0.5f;

    [Header("Camera")]
    [SerializeField] private float _cameraFollowSpeed = 5f;
    [SerializeField] private Vector3 _cameraOffset;

    [Header("Gameplay Constants")]
    [SerializeField] private float _coinCollectionDistance = 1f;
    [SerializeField] private float _projectileLifetime = 5f;
    [SerializeField] private string _obstacleTag = "Obstacle";
    [SerializeField] private int _maxEnemiesCount = 20;
    [SerializeField] private float _enemySpawnDistanceFromCamera = 3f;
    [SerializeField] private float _healthBarHeightOffset = 1.5f;

    [Header("Object Pool Sizes")]
    [SerializeField] private int _projectilePoolSize = 20;
    [SerializeField] private int _enemyPoolSize = 15;
    [SerializeField] private int _coinPoolSize = 10;

    public int PlayerMaxHealth => _playerMaxHealth;
    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float ShootCooldown => _shootCooldown;
    public float ProjectileSpeed => _projectileSpeed;
    public int ProjectileDamage => _projectileDamage;
    public int EnemyMaxHealth => _enemyMaxHealth;
    public float EnemyMoveSpeed => _enemyMoveSpeed;
    public float EnemyStopDistance => _enemyStopDistance;
    public float EnemyDamageDistance => _enemyDamageDistance;
    public int EnemyDamagePerSecond => _enemyDamagePerSecond;
    public float EnemySpawnInterval => _enemySpawnInterval;
    public float CoinDropChance => _coinDropChance;
    public float CameraFollowSpeed => _cameraFollowSpeed;
    public Vector3 CameraOffset => _cameraOffset;

    public float CoinCollectionDistance => _coinCollectionDistance;
    public float ProjectileLifetime => _projectileLifetime;
    public string ObstacleTag => _obstacleTag;
    public int MaxEnemiesCount => _maxEnemiesCount;
    public float EnemySpawnDistanceFromCamera => _enemySpawnDistanceFromCamera;
    public float HealthBarHeightOffset => _healthBarHeightOffset;

    public int ProjectilePoolSize => _projectilePoolSize;
    public int EnemyPoolSize => _enemyPoolSize;
    public int CoinPoolSize => _coinPoolSize;
}
