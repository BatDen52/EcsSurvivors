using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UIEventListeners _uiEventListeners;
    [SerializeField] private Transform _playerPrefab;
    [SerializeField] private Transform _enemyPrefab;
    [SerializeField] private Transform _projectilePrefab;
    [SerializeField] private Transform _coinPrefab;
    [SerializeField] private GameObject _healthBarPrefab;
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

        var poolService = gameObject.AddComponent<PoolService>();
        poolService.Initialize(_projectilePrefab, _enemyPrefab, _coinPrefab, _config);

        var playerFactory = new PlayerFactory(_config, _playerPrefab, _healthBarPrefab);
        var enemyFactory = new EnemyFactory(_config, _enemyPrefab, _mainCamera, _healthBarPrefab);
        var projectileFactory = new ProjectileFactory(_config, _projectilePrefab);
        var coinFactory = new CoinFactory(_config, _coinPrefab);

        var sharedData = new SystemsSharedData {
            SpatialCacheSystem = new SpatialCacheSystem()
        };

        _systems
            .Add(sharedData.SpatialCacheSystem)
            .Add(new SpawnPlayerSystem(playerFactory))
            .Add(new SpawnProjectileSystem(projectileFactory))
            .Add(new SpawnCoinSystem(coinFactory))
            .Add(new PlayerInputSystem(_config))
            .Add(new PlayerMoveSystem())
            .Add(new PlayerShootSystem(_config, _projectilePrefab))
            .Add(new ProjectileMoveSystem(_config))
            .Add(new EnemySpawnSystem(_config, enemyFactory, _mainCamera))
            .Add(new EnemyMoveSystem(_config))
            .Add(new CollisionSystem(_config))
            .Add(new DamageSystem(_config))
            .Add(new HealthCheckSystem())
            .Add(new PlayerDeathSystem())
            .Add(new EnemyDeathSystem(_config))
            .Add(new ProjectileDeathSystem())
            .Add(new ObjectLifecycleSystem(poolService))
            .Add(new CoinCollectSystem(_config))
            .Add(new CameraFollowSystem(_config, _mainCamera))
            .Add(new UIPlayerSystem(_playerHealthText, _coinsText, _gameOverPanel))
            .Add(new UIEnemyHealthSystem(_mainCamera, _config))
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
