using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UIEventListeners _uiEventListeners;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _playerHealthText;
    [SerializeField] private TMPro.TextMeshProUGUI _coinsText;

    private EcsWorld _world;
    private EcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);

        _uiEventListeners.InitializeEcsFilters(_world);

        var projectilePool = new ObjectPool<EntityLink>(_config.ProjectileConfig.ProjectilePrefab, _config.PoolConfig.ProjectilePoolSize);
        var enemyPool = new ObjectPool<EntityLink>(_config.EnemySpawnerConfig.EnemyConfig.EnemyPrefab, _config.PoolConfig.EnemyPoolSize);
        var coinPool = new ObjectPool<EntityLink>(_config.CoinConfig.CoinPrefab, _config.PoolConfig.CoinPoolSize);

        var playerFactory = new PlayerFactory(_config.PlayerConfig);
        var enemyFactory = new EnemyFactory(_config.EnemySpawnerConfig.EnemyConfig, _mainCamera, enemyPool);
        var projectileFactory = new ProjectileFactory(_config.ProjectileConfig, projectilePool);
        var coinFactory = new CoinFactory(_config.CoinConfig, coinPool);

        var sharedData = new SystemsSharedData {
            SpatialCacheSystem = new SpatialCacheSystem()
        };

        _systems
            .Add(sharedData.SpatialCacheSystem)
            .Add(new SpawnPlayerSystem(playerFactory))
            .Add(new SpawnProjectileSystem(projectileFactory))
            .Add(new SpawnCoinSystem(coinFactory))
            .Add(new PlayerInputSystem(new InputService()))
            .Add(new PlayerMoveSystem())
            .Add(new PlayerShootSystem())
            .Add(new ProjectileMoveSystem(_config.ProjectileConfig))
            .Add(new EnemySpawnSystem(_config.EnemySpawnerConfig, enemyFactory, _mainCamera))
            .Add(new EnemyMoveSystem(_config.EnemySpawnerConfig.EnemyConfig))
            .Add(new CollisionSystem(_config.GameplayConfig))
            .Add(new DamageSystem(_config.EnemySpawnerConfig.EnemyConfig))
            .Add(new HealthCheckSystem())
            .Add(new PlayerDeathSystem())
            .Add(new EnemyDeathSystem(_config.CoinConfig))
            .Add(new ProjectileDeathSystem())
            .Add(new ObjectLifecycleSystem(enemyPool, projectilePool, coinPool, _world))
            .Add(new CoinCollectSystem(_config.CoinConfig))
            .Add(new CameraFollowSystem(_config.CameraConfig, _mainCamera))
            .Add(new UIPlayerSystem(_playerHealthText, _coinsText, _gameOverPanel))
            .Add(new UIEnemyHealthSystem(_mainCamera, _config.GameplayConfig))
            .Init();

        var spawnPlayerRequest = _world.NewEntity();
        _world.GetPool<SpawnPlayerRequest>().Add(spawnPlayerRequest);

        var gameState = _world.NewEntity();
        _world.GetPool<GameState>().Add(gameState);
        _world.GetPool<ResourceCoins>().Add(gameState);
    }

    private void Update() =>
        _systems?.Run();

    private void OnDestroy() =>
        _systems?.Destroy();
}
