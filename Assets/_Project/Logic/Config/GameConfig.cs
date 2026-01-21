using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private ProjectileConfig _projectileConfig;
    [SerializeField] private EnemySpawnerConfig _enemySpawnerConfig;
    [SerializeField] private CoinConfig _coinConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [SerializeField] private GameplayConfig _gameplayConfig;
    [SerializeField] private PoolConfig _poolConfig;

    public PlayerConfig PlayerConfig => _playerConfig;
    public ProjectileConfig ProjectileConfig => _projectileConfig;
    public EnemySpawnerConfig EnemySpawnerConfig => _enemySpawnerConfig;
    public CoinConfig CoinConfig => _coinConfig;
    public CameraConfig CameraConfig => _cameraConfig;
    public GameplayConfig GameplayConfig => _gameplayConfig;
    public PoolConfig PoolConfig => _poolConfig;
}
