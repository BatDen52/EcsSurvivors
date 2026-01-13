using Leopotam.EcsLite;
using UnityEngine;

public class CollisionSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly GameConfig _config;
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsFilter _projectileFilter;
    private EcsFilter _enemyFilter;
    private EcsPool<CollisionEvent> _eventsPool;
    private EcsPool<RigidbodyRef> _rigidbodiesPool;
    private EcsPool<ProjectileTag> _projectilePool;
    private EcsPool<EnemyTag> _enemyPool;
    private EcsPool<Health> _healthPool;
    private EcsPool<Damage> _damagePool;
    private EcsPool<TransformRef> _transformsPool;

    public CollisionSystem(GameConfig config)
    {
        _config = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<CollisionEvent>().End();
        _projectileFilter = _world.Filter<ProjectileTag>().Inc<CollisionEvent>().Inc<Damage>().End();
        _enemyFilter = _world.Filter<EnemyTag>().Inc<Health>().End();
        _eventsPool = _world.GetPool<CollisionEvent>();
        _rigidbodiesPool = _world.GetPool<RigidbodyRef>();
        _projectilePool = _world.GetPool<ProjectileTag>();
        _enemyPool = _world.GetPool<EnemyTag>();
        _healthPool = _world.GetPool<Health>();
        _damagePool = _world.GetPool<Damage>();
        _transformsPool = _world.GetPool<TransformRef>();
    }

    public void Run(IEcsSystems systems)
    {
        if (_world == null || !_world.IsAlive())
            return;

        foreach (var projectile in _projectileFilter)
        {
            ref var collisionEvent = ref _eventsPool.Get(projectile);

            if (collisionEvent.Other.TryGetComponent<EntityLink>(out var entityLink))
            {
                var otherEntity = entityLink.Entity;

                if (_enemyPool.Has(otherEntity))
                {
                    ref var enemyHealth = ref _healthPool.Get(otherEntity);
                    enemyHealth.Current -= _damagePool.Get(projectile).Amount;

                    if (enemyHealth.Current < 0)
                        enemyHealth.Current = 0;

                    _world.GetPool<DeadTag>().Add(projectile);
                    _eventsPool.Del(projectile);
                    continue;
                }
            }
            else if (collisionEvent.Other.CompareTag(_config.ObstacleTag))
            {
                _world.GetPool<DeadTag>().Add(projectile);
                _eventsPool.Del(projectile);
                continue;
            }

            _eventsPool.Del(projectile);
        }

        foreach (var entity in _filter)
        {
            ref var collision = ref _eventsPool.Get(entity);

            if (collision.Other.CompareTag(_config.ObstacleTag))
            {
                if (_rigidbodiesPool.Has(entity))
                {
                    _rigidbodiesPool.Get(entity).Value.linearVelocity = Vector3.zero;
                }
            }
            _eventsPool.Del(entity);
        }
    }
}
