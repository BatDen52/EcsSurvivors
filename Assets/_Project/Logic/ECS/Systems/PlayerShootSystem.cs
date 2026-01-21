using Leopotam.EcsLite;
using UnityEngine;

public class PlayerShootSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _playerFilter;
    private EcsFilter _enemyFilter;
    private EcsPool<FireCooldown> _cooldownsPool;
    private EcsPool<TransformRef> _transformsPool;
    private EcsPool<Health> _healthsPool;
    private SpatialCacheSystem _spatialCache;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<FireCooldown>().Inc<TransformRef>().End();
        _enemyFilter = _world.Filter<EnemyTag>().Inc<TransformRef>().End();
        _cooldownsPool = _world.GetPool<FireCooldown>();
        _transformsPool = _world.GetPool<TransformRef>();
        _healthsPool = _world.GetPool<Health>();

        _spatialCache = systems?.GetShared<SystemsSharedData>()?.SpatialCacheSystem;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            if (_healthsPool.Get(player).Current <= 0)
                continue;

            ref var cooldown = ref _cooldownsPool.Get(player);
            cooldown.Current -= Time.deltaTime;

            if (cooldown.Current <= 0)
            {
                cooldown.Current = cooldown.Max;
                Shoot(_world, _transformsPool.Get(player).Value);
            }
        }
    }

    private void Shoot(EcsWorld world, Transform playerTransform)
    {
        Vector3? nearestEnemyPosition = _spatialCache?.GetNearestEnemyPosition(playerTransform.position);
        Vector3 shootDirection;

        if (nearestEnemyPosition.HasValue)
        {
            shootDirection = (nearestEnemyPosition.Value - playerTransform.position).normalized;
        }
        else
        {
            Transform nearestEnemy = null;
            float minSqrDistance = float.MaxValue;

            foreach (var enemy in _enemyFilter)
            {
                ref var enemyTransform = ref _transformsPool.Get(enemy);
                float sqrDistance = playerTransform.position.SqrDistance(enemyTransform.Value.position);

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    nearestEnemy = enemyTransform.Value;
                }
            }

            shootDirection = (nearestEnemy != null) ?
                (nearestEnemy.position - playerTransform.position).normalized :
                playerTransform.forward;
        }

        var spawnRequest = world.NewEntity();
        ref var request = ref world.GetPool<SpawnProjectileRequest>().Add(spawnRequest);
        request.Position = playerTransform.position + shootDirection;
        request.Direction = shootDirection;
    }
}
