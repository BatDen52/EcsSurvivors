using Leopotam.EcsLite;
using UnityEngine;

public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    private readonly EnemyFactory _enemyFactory;
    private readonly Camera _camera;
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsPool<TransformRef> _transformsPool;
    private EcsFilter _spawnRequestFilter;
    private EcsPool<SpawnEnemyRequest> _spawnRequestsPool;
    private EcsPool<EnemySpawnTimer> _spawnTimerPool;
    private int _spawnTimerEntity;

    public EnemySpawnSystem(GameConfig config, EnemyFactory enemyFactory, Camera camera)
    {
        _config = config;
        _enemyFactory = enemyFactory;
        _camera = camera;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformRef>().End();
        _transformsPool = _world.GetPool<TransformRef>();
        _spawnRequestFilter = _world.Filter<SpawnEnemyRequest>().End();
        _spawnRequestsPool = _world.GetPool<SpawnEnemyRequest>();
        _spawnTimerPool = _world.GetPool<EnemySpawnTimer>();

        _spawnTimerEntity = _world.NewEntity();
        ref var spawnTimer = ref _spawnTimerPool.Add(_spawnTimerEntity);
        spawnTimer.Current = 0f;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _spawnRequestFilter)
        {
            ref var request = ref _spawnRequestsPool.Get(entity);
            _enemyFactory.Create(_world, request.Position);
            _world.DelEntity(entity);
        }

        var enemyCount = _world.Filter<EnemyTag>().End().GetEntitiesCount();
        if (enemyCount >= _config.MaxEnemiesCount)
            return;

        ref var spawnTimer = ref _spawnTimerPool.Get(_spawnTimerEntity);
        spawnTimer.Current += Time.deltaTime;

        if (spawnTimer.Current < _config.EnemySpawnInterval)
            return;

        spawnTimer.Current = 0;
        SpawnEnemyOutsideCamera(_world);
    }

    private void SpawnEnemyOutsideCamera(EcsWorld world)
    {
        float cameraHeight = _camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * _camera.aspect;
        Vector3 cameraPos = _camera.transform.position;

        float groundY = 0f;
        foreach (var player in _playerFilter)
        {
            groundY = _transformsPool.Get(player).Value.position.y;
            break;
        }

        Vector3 spawnPosition = FindValidSpawnPosition(cameraHeight, cameraWidth, cameraPos, groundY);

        _enemyFactory.Create(world, spawnPosition);
    }

    private bool IsPositionInsideCameraView(Vector3 position, Camera camera, float margin = 1f)
    {
        var screenPoint = camera.WorldToViewportPoint(position);
        return screenPoint.x > -margin && screenPoint.x < 1 + margin &&
               screenPoint.y > -margin && screenPoint.y < 1 + margin &&
               screenPoint.z > 0;
    }

    private Vector3 FindValidSpawnPosition(float cameraHeight, float cameraWidth, Vector3 cameraPos, float groundY)
    {
        const int maxAttempts = 10;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 position = GeneratePosition(cameraHeight, cameraWidth, cameraPos, groundY);

            if (!IsPositionInsideCameraView(position, _camera))
            {
                return position;
            }
        }

        return GeneratePosition(cameraHeight, cameraWidth, cameraPos, groundY)
               + (_camera.transform.position - GeneratePosition(cameraHeight, cameraWidth, cameraPos, groundY)).normalized
               * _config.EnemySpawnDistanceFromCamera * 2;
    }

    private Vector3 GeneratePosition(float cameraHeight, float cameraWidth, Vector3 cameraPos, float groundY)
    {
        float radius = Mathf.Max(cameraWidth, cameraHeight) / 2 + _config.EnemySpawnDistanceFromCamera * 2;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        Vector3 spawnPosition = new Vector3(
            cameraPos.x + Mathf.Cos(angle) * radius,
            groundY,
            cameraPos.z + Mathf.Sin(angle) * radius
        );

        return spawnPosition;
    }

}
