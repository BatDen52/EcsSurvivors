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
    private EcsPool<SpawnProjectileRequest> _spawnProjectileRequestPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _playerFilter = _world.Filter<PlayerTag>().Inc<FireCooldown>().Inc<TransformRef>().End();
        _enemyFilter = _world.Filter<EnemyTag>().Inc<TransformRef>().End();
        _cooldownsPool = _world.GetPool<FireCooldown>();
        _transformsPool = _world.GetPool<TransformRef>();
        _healthsPool = _world.GetPool<Health>();
        _spawnProjectileRequestPool = _world.GetPool<SpawnProjectileRequest>();

        _spatialCache = systems?.GetShared<SystemsSharedData>()?.SpatialCacheSystem;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            if (_healthsPool.Get(player).Current <= 0)
                continue;

            ref var cooldown = ref _cooldownsPool.Get(player);
            
            if (TryConsumeCooldown(ref cooldown))
            {
                Shoot(_transformsPool.Get(player).Value);
                ResetCooldown(ref cooldown);
            }
        }
    }

    private bool TryConsumeCooldown(ref FireCooldown damageCooldown)
    {
        damageCooldown.Current -= Time.deltaTime;
        return damageCooldown.Current <= 0;
    }

    private void ResetCooldown(ref FireCooldown cooldown)
    {
        cooldown.Current = cooldown.Max;
    }

    private void Shoot(Transform playerTransform)
    {
        Vector3 shootDirection = GetShootDirection(playerTransform.position);
        SpawnProjectile(playerTransform.position + shootDirection, shootDirection);
    }

    private Vector3 GetShootDirection(Vector3 playerPosition)
    {
        Vector3? nearestEnemyPosition = _spatialCache?.GetNearestEnemyPosition(playerPosition);

        if (nearestEnemyPosition.HasValue)
            return (nearestEnemyPosition.Value - playerPosition).normalized;

        return FindNearestEnemyDirection(playerPosition) ?? Vector3.forward;
    }

    private Vector3? FindNearestEnemyDirection(Vector3 playerPosition)
    {
        Transform nearestEnemy = null;
        float minSqrDistance = float.MaxValue;

        foreach (var enemy in _enemyFilter)
        {
            ref var enemyTransform = ref _transformsPool.Get(enemy);
            float sqrDistance = playerPosition.SqrDistance(enemyTransform.Value.position);

            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestEnemy = enemyTransform.Value;
            }
        }

        return nearestEnemy != null
            ? (nearestEnemy.position - playerPosition).normalized
            : null;
    }

    private void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        var spawnRequest = _world.NewEntity();
        ref var request = ref _spawnProjectileRequestPool.Add(spawnRequest);
        request.Position = position;
        request.Direction = direction;
    }
}
